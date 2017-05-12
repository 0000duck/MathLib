# MathLib
.NET math library for working with matrices, functions.

In the class Matrix.cs implemented functionality:
  1. All matrix arithmetic operations;
  2. Get trace of matrix;
  3. Get identity matrix;
  4. Get triangle matrix;
  5. Get args (by Gauss method);
  6. Get reverse matrix;
  7. Get determinant of matrix (by Gauss, Decomposition methods);
  8. Get minor of matrix element.
  9. Get eigen values, vectors of matrix;
  10. Get characteristic polynom of matrix (by Leverrier, Leverrier-Faddeev, Danilevskiy, Krylov methods);
  
In the class Matrix.cs implemented functionality:
  1. Get roots (by Chords, Dichotomy, Golden-Section, Phibonacci methods);
  2. Get integral (by Rectangle, Trapeze, Simpsons methods);
  3. Get differential (by Runge-Kutta method);

Benefits:
  - The ability to perform basic arithmetic operations with matrices with the help of operators;
  - The ability to create function object from usual function string like (x^4 + sin(x) + x^(x+2)) or with delegates. 
  - Rich functionality of classes.
  
Examples:
  1. Matrix operations:
  ```cs
  Matrix m1 = new Matrix(new double[,]{
      {1, 2},
      {3, 4}
  });
  Matrix m2 = new Matrix(new double[,]{
      {5, 6},
      {7, 8}
  });
  
  //sum of 2 matrices
  Matrix sum = m1 + m2;
  
  //difference of 2 matrices
  Matrix sub = m1 - m2;
  
  //multiplicity of 2 matrices
  Matrix multi = m1 * m2;
  
  //reverse matrix of m1
  Matrix reverse = m1.GetReverseMatrix();
  
  ```
  
  2. Functions
  ```cs
  //create by delegate
  Function f1 = new Function(delegate(double x){ return x*x; });
  
  //create by function string
  Function f2 = new Function("x^2 + x + 2");
  
  //create by polynom koef (2*x^3 + 2*x^2 + x + 5)
  Function f3 = new Function(2, 2, 1, 5);
  
  //get roots by dichotomy method
  var roots = f2.GetRootsByDichotomy(-5, 5, 0.0001).ToArray();
  
  ```
