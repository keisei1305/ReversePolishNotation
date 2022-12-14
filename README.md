# ReversePolishNotation
Библиотека, которая позволяет преобразовать математическую запись в форме обратной польской нотации

### На данный момент реализовано:

1.Часто используемые бинарные операторы( +, -, *, /, ^)

2.Стандартные тригонометрические функции, и обратные к ним (sin,arcsin,tg,ctg...)

3.Натуральный логарифм ln


### Parse
Основная функция Parse(), которая возвращает значение математического выражения, в данном случает вернётся __50__
```C#
            string s = "sin(30+60/-2)+2*3^3-4";
            Console.WriteLine(Parse(s,false));
```
Также, если в строке имеется неизвестное(всегда __x__), то можно получить значение выражения от __x__
```C#
            string s = "-x^(x-1)";
            Console.WriteLine(Parse(s,4,false)); //вернётся 64
```

### Рекомендации
При поиске нескольких значение выражения, от разных x, рекомендуется преобразовать строку, в польский вид, а ещё лучше в массив строк,
для улучшения производительности
```C#
            string s = "x+14*3-sin(1)";
            string[] S = MathP.Polska(s);
            for(int i=0; i<3; i++)
            {
                Console.WriteLine(Parse(S, i));
            }
```
Также не рекомендуется ставить между операторами и операндами пробелы, существуют необработанные исключения связанные с ними

### Дальнейшие планы
+ Исправить исключения связанные с пробелами.
+ Добавить популярные префиксные функции, такие как логарифмы от разных оснований
+ Добавить постфиксные функции, ещё нет ни одной функции, что помешает дальнейшей работе
+ Добавить полезные перегрузки, для более простого использования
