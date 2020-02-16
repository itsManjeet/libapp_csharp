using System;
using System.Collections;
using System.Collections.Generic;

namespace app
{
    public delegate int AppFunction(App a);
    public class Author
    {
        string Name,
               Email,
               About;

        public Author(string name, string email, string about)
        {
            this.Name = name;
            this.Email = email;
            this.About = about;
        }

        public string Display()
        {
            return this.Name + " <" + this.Email + "> ";
        }
    }

    public class Sub
    {
        public string Command, Description, Usage;
        public AppFunction AppFunc;

        public Sub(string cmd, string desc, string usage, AppFunction appfunc)
        {
            this.Command = cmd;
            this.Description = desc;
            this.Usage = usage;
            this.AppFunc = appfunc;
        }

        public string Display()
        {
            return this.Command + " " + this.Usage + " : \t\t" + this.Description;
        }
    }
    public class App
    {
        string Name, Version,
               Description,
               Release;
        List<Author> Authors = new List<Author>();
        List<Sub> Subs = new List<Sub>();
        List<string> Flags = new List<string>();
        public List<string> Args = new List<string>();
        AppFunction func = null;

        public App(string name)
        {
            this.Name = name;
        }

        public App version(string ver)
        {
            this.Version = ver;
            return this;
        }

        public App description(string desc)
        {
            this.Description = desc;
            return this;
        }

        public App author(string name, string email, string about)
        {
            Author a = new Author(name, email, about);
            this.Authors.Add(a);
            return this;
        }

        public App sub(string command, string desc, string usage, AppFunction func)
        {
            Sub s = new Sub(command, desc, usage, func);
            this.Subs.Add(s);
            return this;
        }

        public App main_func(AppFunction appfunc)
        {
            this.func = appfunc;
            return this;
        }

        public int print_help()
        {
            Console.WriteLine("%v %v %v", this.Name, this.Version, this.Release);
            foreach(var a in this.Authors)
            {
                Console.Write(a.Display());
            }
            Console.WriteLine("Description: %v", this.Description);
            Console.WriteLine("Usage:\n");
            foreach(var s in this.Subs)
            {
                Console.WriteLine(s.Display());
            }

            return 0;
        }

        public bool isFlagSet(string flag)
        {
            return this.Flags.Contains(flag);
        }
        public int execute(string[] args)
        {
            bool task_found = false;
            Sub Task = null;
            int status = 0;
            if (args.Length <= 1)
            {
                if (this.func == null)
                {
                    status = this.print_help();
                } else
                {
                    status = this.func(this);
                }
                
            } else
            {
                foreach(var a in args)
                {
                    if (a == "--version" || a == "-v" )
                    {
                        Console.WriteLine("Version: ", this.Version);
                        return 0;
                    }

                    if (a == "--help" || a == "-h")
                    {
                        this.print_help();
                        return 0;
                    }

                    if (a[0] == '-')
                    {
                        this.Flags.Add(a);
                        continue;
                    }

                    bool is_task = false;
                    foreach (var s in this.Subs)
                    {
                        if (s.Command == a && ! task_found)
                        {
                            task_found = true;
                            is_task = true;
                            Task = s;
                            break;
                        }
                    }

                    if (!is_task)
                    {
                        this.Args.Add(a);
                    }
                }
            }

            if (task_found)
            {
                status = Task.AppFunc(this);

            } else
            {
                if (this.func != null)
                {
                    this.func(this);
                } else
                {
                    this.print_help();
                }
            }
            return status;
        }
    }
}
