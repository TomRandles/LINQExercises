using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace LINQExercises
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ex 1 
            GetDirFilesWithoutLINQ();
            Console.WriteLine("***");
            Console.WriteLine("***");
            GetDirFilesSortWithLINQ(@"c:\Windows\");

            // Ex 2 - the value of IEnumerable
            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee {Name = "Tom", Id = 3},
                new Employee {Name = "Ann", Id = 4}
            };

            IEnumerable<Employee> sales = new List<Employee>
            {
                new Employee {Name = "Tom", Id = 3},
                new Employee {Name = "Ann", Id = 4}
            };

            //Iterate over list using low level iterator
            IEnumerator<Employee> iterator = developers.GetEnumerator();
            while(iterator.MoveNext())
            {
                Console.WriteLine($"Name {iterator.Current.Name}");
            }

            // Ex3 - Get number of developers with new MyCount extension method
            Console.WriteLine("The number of developers is: {0}", developers.MyCount());

            //Ex 4.1 - print names starting with capital T - use a delegate
            foreach (Employee d in developers.Where(delegate (Employee e)
                                                  { return e.Name.StartsWith("T"); }))
            {
                Console.WriteLine($"Employees beginning with T: {d.Name}");
            }

            // Ex 4.2 - Use a Lambda expression
            foreach (Employee d in developers.Where(e => e.Name.StartsWith("T")))
            {
                Console.WriteLine($"Employees beginning with T: {d.Name}");
            }

            // Ex 5. Write Func using Lambda
            Func<int, int> square = x => x * x;
            Func<int, int, int> multiply = (x,y) => x * y;

            Console.WriteLine("The square of {0} is {1}", 5, square(5));
            Console.WriteLine("{0} multiplied by {1} is {2}", 5, 7, multiply(5,7));

            //5.2 Action - returns void
            Action<int> write = x => Console.WriteLine(x);
            write(square(multiply(3,3)));

            //5.3 Order by and Where - use implicit typing: var instead of Employee
            foreach (var d in developers.Where(e => e.Name.Length == 3 )
                                        .OrderBy(e => e.Name))
            {
                Console.WriteLine($"Employees : {d.Name}");
            }
            // Extract LINQ queries to a variable
            var query = developers.Where(e => e.Name.Length == 3)
                                  .OrderBy(e => e.Name);

            foreach (var d in query)
            {
                Console.WriteLine($"Employees : {d.Name}");
            }

            // 5.6 Rewrite query in Query syntax 
            var query2 = from e in developers
                         where e.Name.Length == 3
                         orderby e.Name
                         select e;

            foreach (var d in query2)
            {
                Console.WriteLine($"Employees : {d.Name}");
            }
        }
        private static void GetDirFilesWithoutLINQ()
        {
            var path = @"c:\Windows\";
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();

            // Sort files
            Array.Sort(files, new FileComparer());

            foreach (var file in files.Take(5))
            {
                Console.WriteLine($"{file.Name,-20} {file.Length,10:N0}");
            }
        }
        private static void GetDirFilesSortWithLINQ(string path)
        {
            //SQL like query
            var query = from file in new DirectoryInfo(path).GetFiles()
                        orderby file.Length descending
                        select file;
            // Or
            // Series of method calls on IEnumerable<T>
            // Included are all explicit generics normally implicit
            var query2 = new DirectoryInfo(path).GetFiles()
                                                .OrderByDescending<FileInfo, long>(f => f.Length)
                                                .Select<FileInfo, FileInfo>(f => f)
                                                .Take<FileInfo>(5);

            foreach (var file in query2)
            {
                Console.WriteLine($"{file.Name,-20} {file.Length,10:N0}");
            }
        }

        public class FileComparer : IComparer<FileInfo>
        {
            public int Compare([AllowNull] FileInfo x, [AllowNull] FileInfo y)
            {
                return x.Length.CompareTo(y.Length);
            }
        }
    }
}