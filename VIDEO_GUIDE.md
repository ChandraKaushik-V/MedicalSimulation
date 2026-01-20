# Video Asset Guide for Medical Simulations

## 📹 Video File Structure

### Where to Put Your Videos

Place all video files in:
```
MedicalSimulation.Web/wwwroot/videos/simulations/
```

### Naming Convention

Follow this exact pattern:
```
{procedure-name}-state-{number}.mp4
```

**Examples:**
- `skin-incision-state-1.mp4`
- `skin-incision-state-2.mp4`
- `appendectomy-state-1.mp4`
- `appendectomy-state-2.mp4`

## 🎬 How to Structure Your Videos

### CRITICAL: Where to Pause for Questions

Each video should show ONE surgical step and end at a natural pause point where a question makes sense.

**Example Flow for "Skin Incision" Procedure:**

#### Video 1: `skin-incision-state-1.mp4` (5-10 seconds)
- **Shows:** Normal, healthy skin with surgical markings
- **Ends at:** Moment before making the incision
- **Question appears:** "What is the first step before making an incision?"
- **User answers → Next video plays**

#### Video 2: `skin-incision-state-2.mp4` (8-12 seconds)
- **Shows:** Scalpel making a clean incision through skin layers
- **Ends at:** Incision complete, wound open
- **Question appears:** "What tool is used to make a precise surgical incision?"
- **User answers → Next video plays**

#### Video 3: `skin-incision-state-3.mp4` (10-15 seconds)
- **Shows:** Needle and thread closing the incision with sutures
- **Ends at:** Wound fully closed
- **Question appears:** "What is the purpose of suturing the incision?"
- **User answers → Simulation complete**

## 📐 Video Specifications

### Required Format
- **Container:** MP4
- **Video Codec:** H.264
- **Resolution:** 1920x1080 (1080p)
- **Frame Rate:** 30 fps
- **Aspect Ratio:** 16:9
- **Audio:** Optional (can be muted or include ambient sounds)

### Recommended Settings
- **Duration:** 5-15 seconds per state
- **Bitrate:** 5-8 Mbps
- **File Size:** Keep under 10MB per video for fast loading

## 🎯 Content Guidelines

### What Makes a Good Surgical Video

1. **Clear Focus:** Show the surgical area clearly
2. **Smooth Motion:** No jerky camera movements
3. **Good Lighting:** Bright, clinical lighting
4. **Appropriate Speed:** Not too fast, not too slow
5. **Natural Pause:** End at a logical stopping point

### Camera Angle
- **Fixed position** (no panning or zooming during the video)
- **Top-down or 45-degree angle** (like a surgeon's view)
- **Close enough to see details** but not so close that context is lost

## 🛠️ Tools for Creating Videos

### Option 1: Blender (Free, Recommended)
- Create 3D medical models
- Animate surgical procedures
- Render as MP4 videos
- Tutorial: https://www.blender.org/

### Option 2: Medical Animation Software
- **BioDigital Human** (web-based)
- **Complete Anatomy** (paid)
- **Visible Body** (educational)

### Option 3: Stock Medical Videos
- **Shutterstock Medical**
- **Getty Images Medical**
- **Pond5 Medical**

### Option 4: Screen Recording from Medical Simulators
- Record from existing medical training software
- Use OBS Studio or similar screen capture
- Edit to appropriate length

## 📝 Current Video Requirements

Based on your seed data, you need **23 videos total**:

### Simulation 1: Simple Skin Incision (3 videos)
- `skin-incision-state-1.mp4` - Normal skin
- `skin-incision-state-2.mp4` - Making incision
- `skin-incision-state-3.mp4` - Suturing

### Simulation 2: Appendectomy (5 videos)
- `appendectomy-state-1.mp4` - Abdomen view
- `appendectomy-state-2.mp4` - Incision made
- `appendectomy-state-3.mp4` - Appendix visible
- `appendectomy-state-4.mp4` - Appendix removed
- `appendectomy-state-5.mp4` - Incision sutured

### Simulation 3: Foreign Object Removal (3 videos)
- `foreign-object-state-1.mp4` - Wound with object
- `foreign-object-state-2.mp4` - Object removed
- `foreign-object-state-3.mp4` - Wound stitched

### Simulation 4: Basic Suturing (4 videos)
- `suturing-state-1.mp4` - Open wound
- `suturing-state-2.mp4` - First stitch
- `suturing-state-3.mp4` - Multiple stitches
- `suturing-state-4.mp4` - Wound closed

### Simulation 5: Simple Heart Procedure (4 videos)
- `heart-state-1.mp4` - Heart visible
- `heart-state-2.mp4` - Incision on heart
- `heart-state-3.mp4` - Blockage removed
- `heart-state-4.mp4` - Heart sutured

### Simulation 6: Tumor Removal (4 videos)
- `tumor-state-1.mp4` - Tissue with tumor
- `tumor-state-2.mp4` - Incision made
- `tumor-state-3.mp4` - Tumor removed
- `tumor-state-4.mp4` - Tissue sutured

## 🚀 Testing Without Real Videos

The system will work even without videos! It will:
1. Try to load your video from `/videos/simulations/`
2. If not found, fall back to placeholder path
3. Show error in console but continue working

You can test the MCQ system immediately and add videos later.

## 💡 Pro Tips

1. **Start Small:** Create videos for just one simulation first (3 videos)
2. **Test Immediately:** See how they look in the system
3. **Iterate:** Adjust timing, angles, lighting based on feedback
4. **Batch Process:** Once you have a good template, create all videos
5. **Compress:** Use HandBrake to reduce file sizes without quality loss

## 🎨 Placeholder Videos (Temporary)

Until you have real videos, the system looks for:
```
/videos/placeholders/placeholder-state-{number}.mp4
```

You can create simple placeholder videos with:
- Text overlays showing state names
- Solid color backgrounds
- 5-second duration
- Any video editing software

## 📧 Need Help?

If you need assistance:
1. Check video file paths match exactly
2. Verify video format is MP4 H.264
3. Test in browser console for loading errors
4. Ensure videos are in `wwwroot/videos/simulations/`
