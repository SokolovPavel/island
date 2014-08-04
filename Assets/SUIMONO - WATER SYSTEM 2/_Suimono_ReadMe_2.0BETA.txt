---------------------------------------------------
SUIMONO: CUSTOMIZABLE WATER SYSTEM
        Copyright ©2014 Tanuki Digital
               Version 2.0 BETA (0.87)
---------------------------------------------------

Thank you for supporting SUIMONO!
if you have any questions, comments, or requests for new features
please visit the Tanuki Digital Forums and post your feedback:

Note: This is a BETA Release and the documentation is still being written.  For integration
advice or any other questions regarding Suimono 2.0, please visit the Tanuki Digital forum.

http://tanukidigital.com/forum/


---------------
BETA NOTES
---------------
BETA of course means that this is “unfinished” software. All features for 2.0 are in place and the purpose of the BETA is to get the code into the hands of as many real-life users as possible... both to identify and eliminate any last minute bugs, but to also judge performance and get essential feedback.

The BETA period is planned to be very short while we put the finishing touches on the code and shaders, and finish the new Suimono 2.0 documentation files.

Normal caveats apply... this is unfinished code and we are not responsible for computer meltdowns, lost productivity, or relationship problems that occur during use of this software... USE AT YOUR OWN RISK!!!! :D 


-----------------
INSTALLATION
-----------------
Version 2.0 is very similar in structure to the current version of Suimono. One of the main updates
however is that, unlike the current version, Suimono Surface objects no longer need to be parented
to the main Module object. This will allow you more flexibility in managing your water surfaces, and
even allow you to attach them to other objects and include them in prefabs! 

IMPORT SUIMONO BASE FILES INTO YOUR PROJECT
Go to: "Assets -> Import Package -> Custom Package..." in the Unity Menu. Then select the
"Suimono_watersystem_v2beta.unitypackage" file.  This will open an import dialog box.
Click the import button and all the Suimono files will be imported into your project list.

ADD THE SUIMONO MODULES TO YOUR SCENE
1) drag the "Suimono_Module" prefab located in the "/PREFABS" folder into your scene list.
2) If it isn't set already, make sure to set the Suimono_Module's position in the transform settings to 0,0,0
3) set the 'Unity Version Target' setting on the Suimono_Module object accordingto your setup.
4) drag the "Suimono_Surface" prefab into your scene list.  This is your water surface.

That's it! you can now position "Water_Surface" anywhere in your scene that you like, and even rename the "Suimono_Surface" object to anything you wish. 

Note for 3.5:
if you're installing into a Unity 3.5 scene, you'll notice a number of errors pop-up in the console on first install. This is because of the included DX11 shaders, which 3.5 doesn't recognize. However these errors should go away after the first install.

Upgrade Note:
Version 2.0 has been almost completely redesigned, as such it will be installed in it's own folder in your project hierarchy. In fact, Suimono and Suimono 2.0 could be run concurrently with each other, though we don't recommend doing so. 

A Note on Pink Water:
If you're getting a pink water plane, this is because the correct shader for your system/setup isn't loading.  For example if you're running DX11 on Windows, and getting pink water, try manually switching the water surface shader to be Suimono2/water_pro_dx11.  Similarly if you're running Unity in Android or iOS mode, try switching the shader to Suimono2/water_mobile, and if using the Free version of Unity, try switching to the mobile shader as well... Suimono2/water_mobile.

Version 2.0 should be detecting and switching these shaders automatically, however this detection does seem to break from time to time on some systems.  We hope to be able to fix this on all systems over the course of the beta.





----------------------------
CUSTOMIZATION NOTES
----------------------------
The Full Suimono 2.0 Documentation File is still being written, however you can use the below notes to help you get the most out of Suimono 2.0...

Defining your Scene Camera
Suimono tracks a scene camera in order to determine when to initiate the underwater switch. Bydefault Suimono will track the scene camera object that has it’s object tag set to “MainCamera”. However, you can customize which camera Suimono tracks by changing the “Main Camera” attribute on the Suimono_Module objects settings before running your scene.

Completely rewritten (and automated) Preset Manager
The Preset Manager has been completely rewritten to be simple to use and automatic. No more copying and pasting code! You can select a preset simply by clicking on it in the list. The “+NEW” button will create a new preset at the bottom of the list, and will fill this preset with the current settings. You can overwrite settings on a preset by pressing the “+” button next to that preset, and delete a preset entirely by pressing the “-” button. You can also rename a preset by pressing the small round button to the left of the preset name, entering the new name, then pressing OK (or X to cancel). 




----------------------
VERSION HISTORY
----------------------
ver.2.0Beta087 - NEW - Added "Use Refraction & Blur FX" option under Performance Settings.  Disabling this improves performance especially on Mac.
	        - NEW - Added "Infinite Scale" attribute.  This controls the size of the infinite ocean tiling under DX9, set this to a larger number to enlarge the detail tile.
	        - CHANGE - Removed "Overall Scale" Attribute.
	        - CHANGE - Improved Refraction and reflection distortion.
	        - CHANGE - Improved reflection compositing.
	        - CHANGE - Reformulated all presets concerning new tweaks.
	        - FIX - "use UV Reversal" setting now works properly when underwater.  Use this setting to fix inadvertently flipped visuals.
	        - FIX - underwater water color now renders as set in the inspector.
	        - FIX - color darkening on refraction and blur layers.  Now refracts exactly what's under the surface.
	        - FIX - setting caustics to "0" freezes Unity, this is now fixed.

ver.2.0Beta086 - NEW - Infinite Ocean now works under DX9. (desktop/weblapyer, Unity Free, Android and iPhone.)
	        - NEW - Updated Wave Normals for better wave and light rendering.
	        - NEW - Added "Overall Brightness" attribute to manually adjust the light/dark levels of the water surface.
	        - NEW - Added parallax to Surface Shaders, for better near-view wave visuals.
	        - CHANGE - Updated shader surface calculations.  Specular now rendered after alpha pass.
	        - CHANGE - Updated depth Rendering, fixes various depth, color, and alpha overlap problems under Unity Pro.
	        - FIX - Incorporated "Overall Transparency" attribute into the preset system.
	        - FIX - Added "Auto Tessellation" setting back to tessellation tab. (was overwritten in previous version)
	        - FIX - pink shader error for Unity Dx11 shader in Unity Free.
	        - FIX - Caustic effects are now working again. 

ver.2.0Beta085 - NEW - Tessellation and 3D wave Displacement now works on Unity Indie (new basic_dx11 shaders added).
	        - NEW - Added "Unity DX11" version target option for Unity Indie.
	        - NEW - Added Overall Transparency attribute, controls the final transparency on water surfaces.
	        - FIX - Added manual ReGenerate() function for Suimono_flowgenerator.js
	        - FIX - Removed custom terrain shader.  This was left in the 8.0 project by mistake, and the removal should fix any interfering issues with terrain.
	        - FIX - Jittery 3d wave scrolling.  Waves should now move smoothly regardless of the speed setting.
	        - FIX - Over-brighten effect when Using 3rd party sky/weather systems.  Removed ambient light brightening in water shaders.
	        - FIX - Specular colors are now influenced by light color.

ver.2.0Beta08 - FIX - Moved initiation elements back into Start() function - dolved intermittent crashing when adding scene prefabs.
HOTFIX          - FIX - reapplied material to surface prefab - solves pink water error in editor.

ver.2.0Beta08 - NEW - Infinite 3D Ocean feature now works on DX11.
	       - NEW - Shoreline Waves calculation implemented (+ 2 new shoreline presets)
	       - NEW - Work-in-progress demo with controllable boat.
	       - NEW - Major performance increases on Android and iOS builds.
	       - NEW - Rewritten iOS and Android shaders.
	       - NEW - FX System component now exposes all particle effects.
	       - NEW - FX Object allows you to place effects and audio anywhere in your scene.
	       - NEW - "Use Dark Skin" option to explicitly switch between dark and light Editor UIs.
	       - NEW - Force Amount setting to buoyancy object.
	       - NEW - "Include Presets in build" option.
	       - NEW - exposed "Auto Tessellation" switches between sized-based tessellation or user specified tessellation.
	       - NEW - Presets and preset transitions can now be accessed in builds.
	       - NEW - Added "Enable Caustics on Mobile" option under the performance tab, this is now off by default.
	       - CHANGE - Presets no longer overwrite Surface Type index setting.
	       - CHANGE - Module and Surface prefabs now automatically disconnect themselves from the prefab reference once in scene.
	       - CHANGE - Default Mesh Collider now simplified for performance.
	       - CHANGE - Default cube reflections now matches demo scene skybox.
	       - CHANGE - Updated surface normal textures for improved wave rendering.
	       - FIX - Improved reflective cubemap rendering when dynamic reflections is diabled.
	       - FIX - Turned depth rendering back on in dx9 shader.
	       - FIX - Flipped Rendering on OSX.
	       - FIX - Preset Transitions no longer throw an error.
	       - FIX - Preset Transitions now morph colors correctly.
	       - FIX - shader references now referenced correctly in builds (removes pink water error).
	       - FIX - dx11 water shader now works properly in build (fixed out-of-range float issue / pink-water error).
	       - FIX - Effects plane no longer clips with occluding objects/terrain.
	       - FIX - surface and refration effects now can blend completely out at shoreline.
	       - FIX - wave displacement sometimes inadvertently stopped functioning.

ver.2.0Beta075 - NEW - Added iOS specific surface shaders.
	       - CHANGE - Added 'Max Vertical Speed' attribute to buoyancy component. (prevents vertical overspeed).
	       - FIX - Flipped water visuals in Forward Rendering mode are now rendering in the correct matrix.
	       - FIX - buoyancy object no longer launches you into the atmosphere at the edge of water surfaces.
	       - FIX - removed forced deferred rendering while underwater.
	       - FIX - water now renders behind billboarded trees, as appropriate.
	       - FIX - Android Build - Pink water surface while underwater.  (Suimono now logs surface shaders for builds).
	       - FIX - Black surface render on iOS taget mode, now fixed.
	       - FIX - missing light attenuation causing squared lights with point and spotlights objects.
	       - FIX - DX11 shader brightness was slightly darker than dx9 version.  fixed.
	       - FIX - Height Function in Linear Color Space is now accurate (effects trnsitions, splash positions, buoyancy etc.)
	       - FIX - Fixed underwater surface visuals in Forward rendering.

ver.2.0Beta07 - NEW - All water shaders have been completely rewritten for visual fidelity and performance improvements.
	       - NEW - Refraction Shift setting mimcs variable color spectrum refraction
	       - NEW - Shoreline edge transparency.
	       - NEW - Detail Wave control settings.
	       - NEW - Shallow water control settings.
	       - NEW - Buoyancy object completely rewritten.
	       - NEW - Water surface speed and direction can now influence position of floating (buoyant) objects.
	       - NEW - Detail Wave control settings.
	       - NEW - Detail Wave control settings.
	       - NEW - Can now set dynamic reflection generation on a per-object basis.
	       - NEW - added infinite ocean setting (experimental... work-in-progress!)
	       - FIX - resinstalled missing components from SuimonoModule prefab, fixing NullReference errors.
	       - CHANGE - Removed rendering path setting.
                     - CHANGE - declare all code references in Awake() rather than Start().
	       - CHANGE - removed Tidal color fx.
	       - CHANGE - Tesellation settings no longer saved with preset.
	       - CHANGE - surface direction and speed no longer saved with preset.

ver.2.0Beta06 - NEW - 'Unity Target Version' setting added to SuimonoModule object.  Manual selection required to explicitly define render states.
                     - FIX - Runtime string errors related to preset load parsing.
                     - FIX - Build export errors trying to access UnityEditor and PlayerSettings.  Builds should now clear all error checks.
                     - FIX - Build Export versions not running preset update data correctly.  Builds should now behave exactly like in-editor.
                     - FIX - Preset system errors preventing the proper saving/loading/renaming of presets.
                     - FIX - Error when deleting the last preset in the list, now fixed.
                     - CHANGE - Underwater fog setting slider no longer goes into the negative.

ver.2.0Beta05 - FIX - tessellation resolution was inexplicably being set too low.  This is now fixed and waves have proper definition.
                     - CHANGE - Updated underwater settings for included presets for better underwater coloring.

ver.2.0Beta04 - FIX - Returned surface mesh object to .fbx format (was mistakenly set as .max, causing errors on import.)
                     - FIX - FX Offset setting now correctly offsets underwater effects plane as expected.
                     - CHANGE - small performance updates to general codebase.
                     - NEW - added caustic object define to the Suimono_Module performance tab.

ver.2.0Beta03 - NEW - Improved underwater depth and fog shading.  Now includes directional-lit fog and edge falloff controls, for a more immersive underwater environment.
                     - NEW - Added Refraction Speed and Refraction Scale attributes to the Underwater Refraction Controls.
                     - NEW - Improved underwater rendering for Android and iOS.  Uses full fog and reflection renderer.
                     - CHANGE - Consolidating vertex shader code into new SuimonoFunctions.cginc library.
                     - CHANGE - Added new "Performance Settings" options to the Suimono Module Object.
                     - CHANGE - Dynamic Reflections now work on mobile by default.  There is a new option to disable Dynamic reflections in the performance settings.
                     - FIX - Dynamic Reflection texture link to main shader was broken.  Now the reflection Object successfully feeds the generated texture to the Surface Shader.
                     - FIX - Blurring now functions correctly on surface object and underwater.

ver.2.0Beta02 - CHANGE - Separated SuimonoObject component and mesh/rendering object for performance reasons.  Surface render is now a child of the master Surface object.
                     - FIX - Automatic Shader switching depending in Unity version and Target Platform.
                     - FIX - general Implicit errors in some preset return functions.
                     - FIX - Unity 4.3 loading wrong UI in Android mode.

ver.2.0Beta01 - Initial Beta Release!







