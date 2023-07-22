# Tree-Generator
Generates procedural trees in the unity game engine

This is my first unity project in which I created a way for trees to be generated procedurally within the unity game engine.
It took me three days in order to figure out how to make the code work properly. The approach I've taken is to generate hexagonal 
segments that move towards a randomised height value. In order for each tree to have variations and not just look like
different heights of the same tree I added a deviation feature in which after a certain point on the tree the trunk
begins to deviate in a random direction along the x and z axis. The last part I added in the tree generation algorithm was then
making the trees become smaller towards the top of the tree, this was fairly simple to implement.

The biggest problem I encountered when creating this algorithm was that textures did not apply correctly to the tree mesh, as because
it was procedurally generated, there could not be a set texture. So in order to combat this I created a triplanar shader, which "projects" the texture onto the 
tree from the three seperate axes (_X, Y and Z_).

Overall i'm happy with how this project turned out considering it was my first attempt at a procedural generation algorithm.
