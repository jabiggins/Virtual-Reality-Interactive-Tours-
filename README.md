# Project Title: Virtual Tours: Experiements in Monooscopic and StereoScopic Virtual Reality



**Website:** https://math.nist.gov/~SRessler/jab15/HomePage/

**Presentation:** https://drive.google.com/file/d/1iBx8SgLyonshZFz6rMDfCCOCirtihpfg/view?usp=sharing

**Objective 1:** Give public access to private spaces

**Objective 2:** Begin to standardize the development process so that non-technical scientists can create their own tours

---

## **Description:** 
I researched and developed virtual reality tours using the Unity Engine. To start, I captured monoscopic and stereoscopic(3D) 360 degree photos and images, and designed a template that could be used to create a virtual tour. I first developed a version that requires a VR headset that can be run on a computer. I have different scripts(C# classes) for the monoscopic and stereoscopic tours. Then, because we wanted to give public access to these spaces, I made a WebVR template that can play on a browser(Firefox works best) and is linked above. The attatched scripts are a part of the stereoscopic WebVR template, which is the most complex template I designed. You can navigate through the tour using either a mouse, or a headset. Through the Unity Editor and utilization of these scripts, a programmer can create an entire tour without writing a single line of code. Use the guide below to understand what each script does and how they interacts with one another.

---

## **Process:**

#### At a location
*AudioTour-*          triggered to play audio

*Signs-*              triggered to display text or pictures

*MenuOptions-*        triggered to switch from tour to tour (PC version only)

#### Simulation Management
*GameManager-*        detects keyboard input

*StateManager2-*      detects whether or not the user switched to VR 

#### Movement between locations
*S_TeleportManager-*  organizes the dissolve transitions and camera movement between locations (stereoscopic version)

*S_SampleFade-*       executes the dissolve transition and map updates, called directly by the teleport manager
