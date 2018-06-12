# Visual Studio Compatibility issues
This project was build with Visual Studio 2017. VS2017 introduced a new project structure, which is not compatible with VS2015. To tackle this issue, we added a branch in which we include the old project structure to make the project compatible with VS2015. The project may not be compatible with VS2013 and below.

## Visual Studio 2017
Just clone the project, open the solution and build.
```
git clone https://github.com/tospie/linked-datapoints.git
```

## Visual Studio 2015
Clone the project, switch to the `vs2015` branch, open the solution and build.
```
git clone https://github.com/tospie/linked-datapoints.git
cd linked-datapoints
git checkout vs2015
```

## Testing
To check, if the project works, set the SimpleResourceServer as your startup project. If the program throws a HTTPListenerException "Access is denied", check if you are allowed to use the ports which are used in Program.cs and change them in the case you dont have access to ports that you have access to.
