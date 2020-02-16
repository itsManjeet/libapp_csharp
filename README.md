# App
    app is a c# library to build console apps that can parse command line arguments, set flags etc.

### Usage
namespace app contains
``` 
int AppFunction(App):  a App function proto type

class Author : to manage information of author's name, email, about

class Sub: to manager Sub Commands of apps

class App: main class to build command line applications

```
### Example Usage
```
using System;
using app;

namespace ExampleApp {
    class ExampleApp {
        static void Main(string[] args) {
            App app = new App("example_app_name");
                app.version("0.1.a")
                   .description("a example implentation of app library")
                   
                   .author("Author1", "author1@mail.com", "main author")
                   .author("Author2", "author2@mail.com", "another author")
                   .author("Author3", "author3@mail.com", "useless guy")

                   .sub("new", "sample command for example app",
                        "[usage for new]", NewFunc)

                    .sub("edit", "edit command for example app",
                        "[usage for edit]", EditFunc);

            int status = app.execute(args);
        }

        static int NewFunc(App a) {
            Console.WriteLine("New Function Calls");
            if (a.isFlagSet("force")) {
                Console.WriteLine("Force flag set");
            }

            var newName = a.Args.at(0);
            Console.WriteLine("new name %s", newName);
            return 0;
        }

        static int EditFunc(App a) {
            if (a.isFlagSet("hard")) {
                Console.WriteLine("hard flag set");
            }

            foreach(var e in a.Args) {
                Console.WriteLine("editing %s", e);
            }
            return 0;
        }
    }
} 

```