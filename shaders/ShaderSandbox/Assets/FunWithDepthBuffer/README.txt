Code in this folder based off of tutorial from https://chrismflynn.wordpress.com/2012/09/06/fun-with-shaders-and-the-depth-buffer/#more-42 and http://williamchyr.com/2013/11/unity-shaders-depth-and-normal-textures/ (used this for "DepthBufferTest")

I actually couldn't get the depth shader to work out of the box and following https://forum.unity.com/threads/need-help-depth-texture-intersection-highlight-shader-on-mobile-android.387692/ ended up buying the scan effect shader from https://www.assetstore.unity3d.com/en/#!/content/47893. I then worked backwards from that shader to fix my current shader. The notes are in the shader "IntersectionTest.shader"


Notes
- Also important to read the tutorial at https://files.unity3d.com/talks/Siggraph2011_SpecialEffectsWithDepth_WithNotes.pdf which the tutorial is based on. The article contains important information about how the depth buffer is stored (for example the distribution is not linear!) and other theory/tutorials. Wow SIGGRAPH tutorials are useful.

- Actually got stuck for a while because all I could see was black. It seems like I couldn't figure out how to acess _CameraDepthTexture, or at least how to get it to show me something other than zeros. In the end this side helped: http://williamchyr.com/2013/11/unity-shaders-depth-and-normal-textures/


Recommended resources for shaders
- Cg programming wikibook is also recommended http://williamchyr.com/2013/11/unity-shader-programming-resources/ 