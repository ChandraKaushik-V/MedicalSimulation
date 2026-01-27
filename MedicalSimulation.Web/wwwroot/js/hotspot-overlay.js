
// Clickable Hotspot Overlay for Video-Based Simulation
class ClickableHotspotOverlay {
    constructor(containerId, videoElement) {
        this.container = document.getElementById(containerId);
        this.video = videoElement;
        this.currentHotspot = null;
        this.isActive = false;
        this.attempts = 0;
        this.isRevealed = false;

        // Multi-hotspot support
        this.hotspots = [];
        this.clickedHotspots = new Set();
        this.isMultiHotspot = false;

        // Ensure container has positioning context so absolute canvas stays inside
        const style = window.getComputedStyle(this.container);
        if (style.position === 'static') {
            this.container.style.position = 'relative';
        }

        // Create overlay canvas
        this.canvas = document.createElement('canvas');
        this.canvas.id = 'hotspot-canvas';
        this.canvas.style.position = 'absolute';
        this.canvas.style.top = '0';
        this.canvas.style.left = '0';
        this.canvas.style.pointerEvents = 'none';
        this.canvas.style.zIndex = '100';
        this.container.appendChild(this.canvas);

        this.ctx = this.canvas.getContext('2d');

        // Resize canvas to match container
        this.resizeCanvas();
        window.addEventListener('resize', () => this.resizeCanvas());
    }

    resizeCanvas() {
        // FIX: Use container dimensions because the video element is detached/hidden (used as texture)
        // and has 0 dimensions.
        const rect = this.container.getBoundingClientRect();

        this.canvas.width = rect.width;
        this.canvas.height = rect.height;
        this.canvas.style.width = rect.width + 'px';
        this.canvas.style.height = rect.height + 'px';

        if (this.isActive && this.currentHotspot) {
            // If revealed, keep showing the answer. Otherwise, show hint if attempts > 0
            if (this.isRevealed) {
                this.showCorrectLocation();
            } else {
                this.drawHotspot(this.attempts >= 1);
            }
        }
    }

    /**
     * Show clickable hotspot overlay
     * @param {Object|Array} hotspotData - Single hotspot {x, y, radius} or array of hotspots
     * @param {Function} onCorrect - Callback when user clicks correctly (all hotspots if multi)
     * @param {Function} onIncorrect - Callback when user clicks incorrectly
     * @param {Function} onMaxAttempts - Callback when max attempts reached (3 failed attempts)
     */
    show(hotspotData, onCorrect, onIncorrect, onMaxAttempts) {
        // Check if this is multi-hotspot (array of hotspots)
        if (Array.isArray(hotspotData)) {
            this.isMultiHotspot = true;
            this.hotspots = hotspotData;
            this.currentHotspot = null;
            this.clickedHotspots = new Set();
        } else {
            this.isMultiHotspot = false;
            this.currentHotspot = hotspotData;
            this.hotspots = [];
            this.clickedHotspots = new Set();
        }

        this.isActive = true;
        this.attempts = 0;
        this.isRevealed = false;
        this.onCorrect = onCorrect;
        this.onIncorrect = onIncorrect;
        this.onMaxAttempts = onMaxAttempts;

        // Enable pointer events on canvas
        this.canvas.style.pointerEvents = 'auto';
        this.canvas.style.cursor = 'crosshair';

        // Draw initial state (usually empty until user interacts or hint is shown)
        this.drawHotspot();

        // Add click listener
        this.canvas.addEventListener('click', this.handleClick.bind(this));
    }

    drawHotspot(showHint = false) {
        // If the location is already revealed, we delegate to showCorrectLocation
        // to prevent clearing the reveal with an empty/hint canvas.
        if (this.isRevealed) {
            this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
            this.showCorrectLocation();
            return;
        }

        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

        if (!showHint) return; // Don't show hint initially

        // Multi-hotspot drawing
        if (this.isMultiHotspot) {
            this.hotspots.forEach((hotspot, index) => {
                const { x, y, radius } = hotspot;
                const pixelX = (x / 100) * this.canvas.width;
                const pixelY = (y / 100) * this.canvas.height;
                const pixelRadius = (radius / 100) * Math.min(this.canvas.width, this.canvas.height);

                // Check if this hotspot has been clicked
                const isClicked = this.clickedHotspots.has(index);

                this.ctx.beginPath();
                this.ctx.arc(pixelX, pixelY, pixelRadius, 0, 2 * Math.PI);

                if (isClicked) {
                    // Already clicked - show as completed
                    this.ctx.strokeStyle = '#10b981';
                    this.ctx.lineWidth = 4;
                    this.ctx.stroke();
                    this.ctx.fillStyle = 'rgba(16, 185, 129, 0.3)';
                    this.ctx.fill();

                    // Draw checkmark
                    this.ctx.font = 'bold 24px Arial';
                    this.ctx.fillStyle = '#10b981';
                    this.ctx.textAlign = 'center';
                    this.ctx.textBaseline = 'middle';
                    this.ctx.fillText('✓', pixelX, pixelY);
                } else {
                    // Not clicked yet - show as hint
                    this.ctx.strokeStyle = 'rgba(16, 185, 129, 0.6)';
                    this.ctx.lineWidth = 3;
                    this.ctx.stroke();
                    this.ctx.fillStyle = 'rgba(16, 185, 129, 0.2)';
                    this.ctx.fill();
                }
            });
            return;
        }

        // Single hotspot drawing
        const { x, y, radius } = this.currentHotspot;

        // Convert percentage coordinates to pixels
        const pixelX = (x / 100) * this.canvas.width;
        const pixelY = (y / 100) * this.canvas.height;
        const pixelRadius = (radius / 100) * Math.min(this.canvas.width, this.canvas.height);

        // Draw pulsing circle hint
        this.ctx.beginPath();
        this.ctx.arc(pixelX, pixelY, pixelRadius, 0, 2 * Math.PI);
        this.ctx.strokeStyle = 'rgba(16, 185, 129, 0.6)';
        this.ctx.lineWidth = 3;
        this.ctx.stroke();

        // Draw inner fill
        this.ctx.fillStyle = 'rgba(16, 185, 129, 0.2)';
        this.ctx.fill();
    }

    handleClick(event) {
        if (!this.isActive) return;

        const rect = this.canvas.getBoundingClientRect();
        const clickX = event.clientX - rect.left;
        const clickY = event.clientY - rect.top;

        // Multi-hotspot handling
        if (this.isMultiHotspot) {
            let clickedAny = false;

            this.hotspots.forEach((hotspot, index) => {
                // Skip if already clicked
                if (this.clickedHotspots.has(index)) return;

                const { x, y, radius } = hotspot;
                const pixelX = (x / 100) * this.canvas.width;
                const pixelY = (y / 100) * this.canvas.height;
                const pixelRadius = (radius / 100) * Math.min(this.canvas.width, this.canvas.height);

                const distance = Math.sqrt(
                    Math.pow(clickX - pixelX, 2) +
                    Math.pow(clickY - pixelY, 2)
                );

                if (distance <= pixelRadius) {
                    // Correct click on this hotspot!
                    clickedAny = true;
                    this.clickedHotspots.add(index);
                }
            });

            if (clickedAny) {
                // Immediately redraw to show the clicked hotspot with checkmark
                this.drawHotspot(this.attempts >= 1);

                // Check if all hotspots are clicked
                if (this.clickedHotspots.size === this.hotspots.length) {
                    this.showMessage('All points identified correctly!', '#10b981');
                    setTimeout(() => {
                        this.hide();
                        if (this.onCorrect) this.onCorrect();
                    }, 1500);
                } else {
                    this.showMessage(`${this.clickedHotspots.size}/${this.hotspots.length} points identified`, '#3b82f6');
                }
            } else {
                // Incorrect click
                this.attempts++;
                this.showFeedback(false, clickX, clickY);

                if (this.onIncorrect) this.onIncorrect();

                // Check if max attempts reached
                if (this.attempts >= 3) {
                    this.showMessage('Correct answer revealed', '#ef4444');
                    setTimeout(() => {
                        this.revealCorrectLocation();
                        setTimeout(() => {
                            this.hide();
                            if (this.onMaxAttempts) this.onMaxAttempts();
                        }, 2000);
                    }, 1500);
                }
            }
            return;
        }

        // Single hotspot handling
        const { x, y, radius } = this.currentHotspot;
        const pixelX = (x / 100) * this.canvas.width;
        const pixelY = (y / 100) * this.canvas.height;
        const pixelRadius = (radius / 100) * Math.min(this.canvas.width, this.canvas.height);

        // Calculate distance from click to hotspot center
        const distance = Math.sqrt(
            Math.pow(clickX - pixelX, 2) +
            Math.pow(clickY - pixelY, 2)
        );

        if (distance <= pixelRadius) {
            // Correct click!
            this.showFeedback(true, clickX, clickY);
            setTimeout(() => {
                this.hide();
                if (this.onCorrect) this.onCorrect();
            }, 1500);
        } else {
            // Incorrect click
            this.attempts++;
            this.showFeedback(false, clickX, clickY);

            // Notify parent, but allow retries internally
            if (this.onIncorrect) this.onIncorrect();

            // Check if max attempts reached
            if (this.attempts >= 3) {
                this.showMessage('Correct answer revealed', '#ef4444');
                setTimeout(() => {
                    this.revealCorrectLocation();
                    setTimeout(() => {
                        this.hide();
                        if (this.onMaxAttempts) this.onMaxAttempts();
                    }, 2000);
                }, 1500);
            }
        }
    }

    showFeedback(isCorrect, x, y) {
        // Draw checkmark or X at click location
        this.ctx.font = 'bold 48px Arial';
        this.ctx.textAlign = 'center';
        this.ctx.textBaseline = 'middle';

        if (isCorrect) {
            this.ctx.fillStyle = '#10b981';
            this.ctx.fillText('✓', x, y);

            // Show success message
            const message = (this.currentHotspot && this.currentHotspot.correctFeedback) || 'Correct!';
            this.showMessage(message, '#10b981');
        } else {
            this.ctx.fillStyle = '#ef4444';
            this.ctx.fillText('✗', x, y);

            // Show error message
            const message = (this.currentHotspot && this.currentHotspot.incorrectFeedback) || 'Try again. Look carefully!';
            this.showMessage(message, '#ef4444');
        }
    }

    showCorrectLocation() {
        // Multi-hotspot reveal
        if (this.isMultiHotspot) {
            this.hotspots.forEach((hotspot) => {
                const { x, y, radius } = hotspot;
                const pixelX = (x / 100) * this.canvas.width;
                const pixelY = (y / 100) * this.canvas.height;

                // Draw only the target icon, no circle
                this.ctx.font = 'bold 36px Arial';
                this.ctx.fillStyle = '#10b981';
                this.ctx.textAlign = 'center';
                this.ctx.textBaseline = 'middle';
                this.ctx.fillText('🎯', pixelX, pixelY);
            });
            return;
        }

        // Single hotspot reveal
        if (!this.currentHotspot) return;

        const { x, y, radius } = this.currentHotspot;
        const pixelX = (x / 100) * this.canvas.width;
        const pixelY = (y / 100) * this.canvas.height;

        // Draw only the target icon, no circle
        this.ctx.font = 'bold 36px Arial';
        this.ctx.fillStyle = '#10b981';
        this.ctx.textAlign = 'center';
        this.ctx.textBaseline = 'middle';
        this.ctx.fillText('🎯', pixelX, pixelY);
    }

    showMessage(text, color) {
        // Create or update message overlay
        let messageEl = document.getElementById('hotspot-message');
        if (!messageEl) {
            messageEl = document.createElement('div');
            messageEl.id = 'hotspot-message';
            messageEl.style.position = 'absolute';
            messageEl.style.top = '20px';
            messageEl.style.left = '50%';
            messageEl.style.transform = 'translateX(-50%)';
            messageEl.style.padding = '12px 24px';
            messageEl.style.borderRadius = '8px';
            messageEl.style.fontWeight = 'bold';
            messageEl.style.fontSize = '16px';
            messageEl.style.zIndex = '20';
            messageEl.style.maxWidth = '80%';
            messageEl.style.textAlign = 'center';
            this.container.appendChild(messageEl);
        }

        messageEl.textContent = text;
        // Black background with white text for all messages
        messageEl.style.backgroundColor = '#000000';
        messageEl.style.color = '#ffffff';
        messageEl.style.border = 'none';
        messageEl.style.display = 'block';

        // Auto-hide after 3 seconds
        setTimeout(() => {
            messageEl.style.display = 'none';
        }, 3000);
    }

    // Public method to reveal the correct location (called from hint button)
    revealCorrectLocation() {
        console.log('revealCorrectLocation called', {
            isActive: this.isActive,
            isMultiHotspot: this.isMultiHotspot,
            currentHotspot: this.currentHotspot,
            hotspots: this.hotspots,
            canvasWidth: this.canvas.width,
            canvasHeight: this.canvas.height
        });

        if (!this.isActive || (!this.currentHotspot && !this.isMultiHotspot)) {
            console.warn('Cannot reveal: not active or no hotspot');
            return;
        }

        this.isRevealed = true;
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.showCorrectLocation();

        const message = this.isMultiHotspot
            ? 'Correct location revealed!'
            : 'Correct location revealed!';
        this.showMessage(message, '#3b82f6');

        console.log('Correct location revealed:', this.isMultiHotspot ? this.hotspots : this.currentHotspot);
    }

    hide() {
        this.isActive = false;
        this.currentHotspot = null;
        this.isRevealed = false;
        this.isMultiHotspot = false;
        this.hotspots = [];
        this.clickedHotspots = new Set();
        this.canvas.style.pointerEvents = 'none';
        this.canvas.style.cursor = 'default';
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

        // Remove click listener
        this.canvas.removeEventListener('click', this.handleClick);

        // Hide message
        const messageEl = document.getElementById('hotspot-message');
        if (messageEl) {
            messageEl.style.display = 'none';
        }
    }
}