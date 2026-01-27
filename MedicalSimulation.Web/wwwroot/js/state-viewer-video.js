class StateViewerVideo {
    constructor(containerId, states, simulationId, progressId) {
        this.container = document.getElementById(containerId);
        if (!this.container) {
            throw new Error(`Container ${containerId} not found`);
        }

        this.simulationId = simulationId;
        this.progressId = progressId;

        // Sort states ONCE by pauseTime
        this.states = states
            .map(s => ({
                ...s,
                hotspotData: this.safeParseJSON(s.hotspotDataJson),
                answerOptions: this.safeParseJSON(s.answerOptionsJson),
                pauseTime: this.extractPauseTime(s)
            }))
            .sort((a, b) => a.pauseTime - b.pauseTime);

        this.currentStateIndex = 0;
        this.currentScore = 0;
        this.pointsPerQuestion = 20; // 6 questions × 20 = 120 total
        this.scoredStates = new Set(); // Track which states have been scored

        this.video = null;
        this.overlayContainer = null;
        this.hotspotOverlay = null;

        this.isTransitioning = false;

        // Timer properties
        this.startTime = null;
        this.timerInterval = null;
    }

    /* ---------------- INIT ---------------- */

    initialize() {
        this.setupDOM();
        this.loadVideo();
        this.startTimer();
    }

    setupDOM() {
        this.container.innerHTML = '';
        this.container.style.position = 'relative';

        this.video = document.createElement('video');
        this.video.style.width = '100%';
        this.video.style.height = '100%';
        this.video.style.objectFit = 'cover';
        this.video.playsInline = true;
        this.video.controls = false;

        this.container.appendChild(this.video);

        this.overlayContainer = document.createElement('div');
        this.overlayContainer.style.position = 'absolute';
        this.overlayContainer.style.inset = '0';
        this.overlayContainer.style.pointerEvents = 'none';
        this.container.appendChild(this.overlayContainer);

        if (typeof ClickableHotspotOverlay !== 'undefined') {
            this.hotspotOverlay = new ClickableHotspotOverlay(
                this.container.id,
                this.video
            );
        }
    }

    loadVideo() {
        const firstState = this.states[0];
        if (!firstState || !firstState.videoUrl) {
            throw new Error('No video URL found');
        }

        this.video.src = firstState.videoUrl;

        this.video.addEventListener('loadedmetadata', () => {
            this.goToState(0);
        });
    }

    /* ---------------- STATE CONTROL ---------------- */

    goToState(index) {
        if (index >= this.states.length) {
            this.finishSimulation();
            return;
        }

        this.currentStateIndex = index;
        const state = this.states[index];

        this.isTransitioning = true;
        this.updateUI(state);

        // For the first state, seek to 6 seconds instead of the beginning
        if (index === 0) {
            this.video.currentTime = 6;
        }
        // Otherwise, continue playing from current position

        // Set up listener to pause at the target pause time
        const targetPauseTime = state.pauseTime;

        const onTimeUpdate = () => {
            if (this.video.currentTime >= targetPauseTime) {
                this.video.pause();
                this.video.removeEventListener('timeupdate', onTimeUpdate);
                this.isTransitioning = false;
                this.showInteraction(state);
            }
        };

        this.video.addEventListener('timeupdate', onTimeUpdate);
        this.video.play();
    }

    /* ---------------- INTERACTIONS ---------------- */

    showInteraction(state) {
        const instructions = document.getElementById('stepInstructions');
        if (instructions) {
            instructions.innerHTML =
                `<strong>${state.stateName}</strong><br>${state.questionText || state.description}`;
        }

        if (state.interactionType === 'click-hotspot') {
            this.showHotspot(state);
        } else if (state.interactionType === 'mcq') {
            this.showMCQ(state);
        } else {
            // Fallback: auto-advance
            setTimeout(() => this.onInteractionSuccess(), 800);
        }
    }

    showHotspot(state) {
        if (!this.hotspotOverlay || !state.hotspotData) {
            this.onInteractionSuccess();
            return;
        }

        this.overlayContainer.style.pointerEvents = 'auto';

        // Check if hotspotData is an array (multi-hotspot) or single object
        const hotspotData = state.hotspotData;

        this.hotspotOverlay.show(
            hotspotData,
            () => {
                // User answered correctly - award points
                this.overlayContainer.style.pointerEvents = 'none';
                this.addScoreForState(state.id);
                this.onInteractionSuccess();
            },
            () => { },
            () => {
                // Max attempts reached - do NOT award points
                this.overlayContainer.style.pointerEvents = 'none';
                this.onInteractionSuccess();
            }
        );
    }

    showMCQ(state) {
        const overlay = document.getElementById('mcqOverlay');
        const question = document.getElementById('questionText');
        const answers = document.getElementById('answersContainer');
        const feedbackOverlay = document.getElementById('feedbackOverlay');
        const feedbackText = document.getElementById('feedbackText');

        if (!overlay || !question || !answers) {
            this.onInteractionSuccess();
            return;
        }

        question.textContent = state.questionText;
        answers.innerHTML = '';

        // Track attempts for this MCQ
        let mcqAttempts = 0;

        state.answerOptions.forEach((opt, idx) => {
            const btn = document.createElement('button');
            btn.className = 'mcq-answer-btn'; // Add the CSS class
            btn.textContent = opt;
            btn.onclick = () => {
                if (idx === state.correctAnswerIndex) {
                    // Correct answer - show success feedback
                    btn.style.background = 'rgba(16, 185, 129, 0.5)';
                    btn.style.borderColor = '#10b981';

                    // Show feedback overlay
                    if (feedbackOverlay && feedbackText) {
                        feedbackOverlay.innerHTML = '<i class="fas fa-check-circle"></i><p>Correct! Well done!</p>';
                        feedbackOverlay.classList.remove('hidden');
                    }

                    // Disable all buttons to prevent multiple clicks
                    answers.querySelectorAll('button').forEach(b => b.disabled = true);

                    // Wait a moment before hiding and advancing
                    setTimeout(() => {
                        overlay.classList.add('hidden');
                        if (feedbackOverlay) feedbackOverlay.classList.add('hidden');
                        this.addScoreForState(state.id);
                        this.onInteractionSuccess();
                    }, 1500);
                } else {
                    // Wrong answer - increment attempts
                    mcqAttempts++;

                    // Show error feedback
                    btn.style.background = 'rgba(239, 68, 68, 0.5)';
                    btn.style.borderColor = '#ef4444';

                    if (mcqAttempts >= 3) {
                        // Max attempts reached - reveal correct answer
                        if (feedbackOverlay && feedbackText) {
                            feedbackOverlay.innerHTML = '<i class="fas fa-exclamation-circle"></i><p>Maximum attempts reached. Revealing correct answer...</p>';
                            feedbackOverlay.classList.remove('hidden');
                        }

                        // Disable all buttons
                        answers.querySelectorAll('button').forEach(b => b.disabled = true);

                        // Highlight the correct answer
                        setTimeout(() => {
                            const correctBtn = answers.children[state.correctAnswerIndex];
                            if (correctBtn) {
                                correctBtn.style.background = 'rgba(16, 185, 129, 0.5)';
                                correctBtn.style.borderColor = '#10b981';
                            }

                            // Advance without scoring
                            setTimeout(() => {
                                overlay.classList.add('hidden');
                                if (feedbackOverlay) feedbackOverlay.classList.add('hidden');
                                this.onInteractionSuccess();
                            }, 2500);
                        }, 1500);
                    } else {
                        // Still have attempts left
                        if (feedbackOverlay && feedbackText) {
                            feedbackOverlay.innerHTML = `<i class="fas fa-exclamation-circle"></i><p>Incorrect. Try again! (${mcqAttempts}/3 attempts)</p>`;
                            feedbackOverlay.classList.remove('hidden');
                        }

                        // Shake animation
                        btn.style.animation = 'shake 0.5s';

                        // Reset button after a moment
                        setTimeout(() => {
                            btn.style.background = '';
                            btn.style.borderColor = '';
                            btn.style.animation = '';
                            if (feedbackOverlay) feedbackOverlay.classList.add('hidden');
                        }, 1500);
                    }
                }
            };
            answers.appendChild(btn);
        });

        overlay.classList.remove('hidden');
    }

    /* ---------------- ADVANCE ---------------- */

    onInteractionSuccess() {
        if (this.isTransitioning) return;

        const nextIndex = this.currentStateIndex + 1;
        this.goToState(nextIndex);
    }

    /* ---------------- FINISH ---------------- */

    finishSimulation() {
        console.log('Simulation complete');

        // Stop the timer
        this.stopTimer();

        // Save final score to server
        this.saveScore();

        // Check if the last state has an endTime specified
        const lastState = this.states[this.states.length - 1];
        const endTime = lastState?.hotspotData?.endTime;

        if (endTime && typeof endTime === 'number') {
            // Play the video until the specified end time
            this.video.play();

            const onTimeUpdate = () => {
                if (this.video.currentTime >= endTime) {
                    this.video.pause();
                    this.video.removeEventListener('timeupdate', onTimeUpdate);
                    // Redirect to results page
                    window.location.href = `/Simulations/Results/${this.progressId}`;
                }
            };

            this.video.addEventListener('timeupdate', onTimeUpdate);
        } else {
            // Play the video to the end (original behavior)
            this.video.play();

            // When video ends, redirect to results page
            this.video.onended = () => {
                window.location.href = `/Simulations/Results/${this.progressId}`;
            };
        }
    }

    /* ---------------- HELPERS ---------------- */

    extractPauseTime(state) {
        const data = this.safeParseJSON(state.hotspotDataJson);

        // Handle array format (multi-hotspot)
        if (Array.isArray(data) && data.length > 0 && typeof data[0].pauseTime === 'number') {
            return data[0].pauseTime;
        }

        // Handle single object format
        if (data && typeof data.pauseTime === 'number') {
            return data.pauseTime;
        }

        return 0;
    }

    safeParseJSON(json) {
        try {
            return json ? JSON.parse(json) : null;
        } catch {
            return null;
        }
    }

    updateUI(state) {
        const current = document.getElementById('currentStep');
        const total = document.getElementById('totalSteps');

        if (current) current.textContent = this.currentStateIndex + 1;
        if (total) total.textContent = this.states.length;
    }

    addScore(points) {
        this.currentScore += points;
        const scoreElement = document.getElementById('currentScore');
        if (scoreElement) {
            scoreElement.textContent = this.currentScore;
        }
    }

    addScoreForState(stateId) {
        // Only add score if this state hasn't been scored yet
        if (!this.scoredStates.has(stateId)) {
            this.scoredStates.add(stateId);
            this.addScore(this.pointsPerQuestion);
        }
    }

    async saveScore() {
        try {
            const response = await fetch('/Simulations/UpdateScore', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    progressId: this.progressId,
                    score: this.currentScore
                })
            });

            if (!response.ok) {
                console.error('Failed to save score');
            }
        } catch (error) {
            console.error('Error saving score:', error);
        }
    }

    /* ---------------- TIMER ---------------- */

    startTimer() {
        this.startTime = Date.now();
        this.timerInterval = setInterval(() => {
            this.updateTimer();
        }, 1000);
    }

    updateTimer() {
        if (!this.startTime) return;

        const elapsed = Math.floor((Date.now() - this.startTime) / 1000);
        const minutes = Math.floor(elapsed / 60);
        const seconds = elapsed % 60;

        const timeElement = document.getElementById('timeElapsed');
        if (timeElement) {
            timeElement.textContent =
                `${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
        }
    }

    stopTimer() {
        if (this.timerInterval) {
            clearInterval(this.timerInterval);
            this.timerInterval = null;
        }
    }

    /* ---------------- HINT & EXIT ---------------- */

    showHint() {
        const currentState = this.states[this.currentStateIndex];
        if (!currentState) return;

        // Show a hint overlay
        const instructions = document.getElementById('stepInstructions');
        if (instructions && currentState.correctAnswerIndex !== undefined) {
            const correctAnswer = currentState.answerOptions[currentState.correctAnswerIndex];
            instructions.innerHTML =
                `<strong>${currentState.stateName}</strong><br>${currentState.questionText}<br><br>` +
                `<span style="color: #4ade80; font-weight: 600;"><i class="fas fa-lightbulb"></i> Hint: The correct answer is "${correctAnswer}"</span>`;
        } else if (instructions && currentState.hotspotData) {
            instructions.innerHTML =
                `<strong>${currentState.stateName}</strong><br>${currentState.questionText}<br><br>` +
                `<span style="color: #4ade80; font-weight: 600;"><i class="fas fa-lightbulb"></i> Hint: Look for the highlighted area on the video</span>`;
        }
    }

    exit() {
        if (confirm('Are you sure you want to exit the simulation? Your progress will be saved.')) {
            this.stopTimer();
            window.location.href = '/Specialties';
        }
    }
}
