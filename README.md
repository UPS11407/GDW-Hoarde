# GDW Hoarde
 
 Second Year GDW Project - Ontario Tech University
 
**Computer Graphics Links:**  
Slides/Presentation: https://docs.google.com/presentation/d/1NFASQ3cFClaRGrcFFls1AlGgtkelJdcnCQDGvHQe_uA/edit#slide=id.p
Trello/Planner: https://trello.com/invite/b/7Xf5ASdL/ATTI789cda3810159fbbbe0fb832bbe51c80ACC868BA/computer-graphics-planning  
Group Screen Capture: https://media.discordapp.net/attachments/933432174605123594/1065041380327575552/MNC.PNG?width=1440&height=173  
Presentation Video: https://youtu.be/gJ7QWGfTnxA 

Members in Computer Graphics:  
Alexander Phillips: A multi-faceted dev that has worked on various programming, design but mainly art contributions to the game.  
Ethan Zafra: Mainly a programmer that works on the game's gameplay scripts. In the previous semester, I've tried working on lighting to improve the level's visual appearance.  

Contribution for group assignment:\
Alexander Phillips: Character Rim Lighting, Vertex Extrusion, Outlines, Particles and Texture Normal Maps\
Ethan Zafra: Enemy Decals, Night Vision, Scan Lines, Lens Distorion, Color Grading and Specular Illumination.

Group Assignment explanation:\
Decals: Decals were added on top of the main enemy texture when a bool is true. A seperate script that is used for the enemy behavior turns on the bool when the enemy is damaged.\
Texture filtering: Used a 2x2 box filter that takes the average value of the nearest 2x2 pixels to determine the value for a pixel.\
Character Outlines: Outlines with a main texture image were added as a replacement for the standard shader. The amount of extrusion for the outline is larger due to the models.\
Particles: The default Unity particle system was used in conjunction with the rim lighting learned in class. The particles would trigger from a separate C# script upon enemy death.\
Shadows: The shadows were made through a shader that was placed onto certain objects. To brighten the objects, the objects colour value was multiplied by 5.

Graphics Controls:\
Numpad 0: Toggle Wall Texture\
Numpad 1: Toggle Color Grading\
Numpad 2: Switch Color Grading

Outside References:
Blood decal 2D texture: https://opengameart.org/content/filth-texture-set-trakrustdecal1tga. Used this as a placeholder texture for the decal for when the enemy is damaged, since I'm horrible at drawing. It will be replaced once our 2D artist makes a blood splatter texture.
