# Universal Grid

The UniversalGrid is a stand-alone library for managing objects within a 2 dimensional grid.

I wrote this library after the need arose on numerous occations for a simple, light-weight grid for managing objects in this manner. Sometimes, grids are useful when 
creating simple WPF board games, in web applications for laying out tables and for many other applications.

## Concepts

The grid represents a constrained 2 dimensional cartesian space. Objects can be added to this space and moved within the space. 

The grid takes care of basic logical itegrity within the space. For example, items can't be moved beyond the bounds of the grid.

In addition to the in-built rules, additional rules can be defined and added to the grid to contrain the movement of items within the grid.

## Examples

### Basic usuage

```cs

var grid = new UniversalGrid<string>(10, 20);

var thing1 = "A".AsSpatialObject(1, 1);

grid.ItemMoved += (s, e) =>
{
	// listen into changes in the objects position
};

grid.SetObject(thing1);

var items = grid.GetObjectsAt(1, 1);

thing1.Move(0, 1);

```

### Setting up rules

```cs

var thing1 = "A".AsSpatialObject(1, 1);

grid.RuleViolated += (s, e) =>
{
    // track violates
};

grid.AddMovementRule((x, m) => m.Any(p => p.Y > 2), 1, 23); // Add a rule which prevents Y from exceeding 2

grid.SetObject(thing1);

thing1.Move(Direction.Down); // If movement violates the rule, it will be prevented and an event will be fired

```

See unit tests for more examples