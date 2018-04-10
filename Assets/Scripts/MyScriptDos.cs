using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MyScriptDos : MonoBehaviour
{
    int playCounter, /**/ counterColorH, /**/ counterColorV, /**/ counterColorRD, /**/ counterColorLD, /**/ noWinnerCount;      //En esta línea declaro todas las variables de tipo "int" que harán la función de contador en el código y verificarán cuando alguien ganó, número de jugadas y empate. Además se inicializan por defecto, es decir, cero.
    int width = 10; /**/ int height = 10;                                                                                       //Declaro dos variables de tipo "int" que servirán para definir el tamaño del tablero, que en este caso será de 10 X 10.
    int redPoints = 0; /**/ int bluePoints = 0;                                                                                 //Declaro dos variables de tipo "int" que servirán para llevar el conteo de victorias de cada jugador.
    float timeLeft;                                                                                                             //Declaro una variable de tipo "float" que se inicializa por defecto en 0.
    bool loopOutput = false; /**/ bool youCanPaint = true;                                                                      //Defino dos variables de tipo "bool", que una se inicializa como "false" (falso) y la otra como "true" (Verdadero).
    public GameObject puzzlePiece;                                                                                              //Declaro una variable de tipo "GameObject" que será la pieza u objeto del que se conformará el tablero, para hacerlo se agrega al ispector de Unity ya que la variable es "public" (pública).
    public GameObject changeColor;                                                                                              //Declaro una variable pública de tipo "GameObject", una para manejar el texto de cambio de color.
    GameObject[,] grid;                                                                                                         //Creo la matriz de dos dimensiones "[,]" y digo que será una matriz de "GameObject", es decir, que en la matriz se llenará con "GameObejects".
    Color playerOne = Color.red; /**/ Color playerTwo = Color.blue;                                                             //Declaro dos variables de tipo color que manejará a ambos jugadores con sus respectivos colores.
    Color standartColor = Color.white; /**/ Color signalColor = Color.black;                                                    //Declaro dos variables de tipo color que se encargarán de dos funciones, una servirá para que la esfera señalada se pinte (Ayuda visual), y las que no, sean del color por defecto (En este caso blanco).
    public Text redText; /**/ public Text blueText;                                                                             //Declaro dos variables públicas de tipo "text" que serán dos textos y estos se irán modificando dependiendo de las variables que controlan la puntuación o victorias. 
    public GameObject redTurn; /**/ public GameObject blueTurn;                                                                 //Estos "GameObject" serán utilizados para mostrar de quién es el turno.

    void Start()                                                                        //En la función "Start" escribo el bloque de código que se debe ejecutar apenas inicie el juego, y es el siguiente.
    {
        timeLeft = Random.Range(15f, 25f);                                              //Inicializo la variable "timeLeft" anteriormente creada como un valor "Random" (aleatorio), dentro de un rango que va desde 20, hasta 30, es decir que la variable puede tomar cualquier valor dentro de este rango
        grid = new GameObject[width, height];                                           //Se da el tamaño a la matriz, en donde la cantidad de columnas será igual a "width" (Variable anteriormente creada que tiene como valor 10) y la cantidad de filas será igual a "height" (Variable anteriormente creada que tiene como valor 10).
        redTurn.GetComponent<Animator>().enabled = true;

        for (int x = 0; x < width; x++)                                                 //Creo un bucle "for" en donde la condición de los paréntesis es así: Para un "int" de nombre "x" hasta que "x" sea menor que "width", "x" se vaya incrementando en 1 cada vuelta, es decir, si "x" llega a ser mayor que "width", no entrará al siguiente "for". 
        {
            for (int y = 0; y < height; y++)                                            //Creo otro bucle "for" en donde la condición de los paréntesis es así: Para un "int" de nombre "y" hasta que "y" sea menor que "height", "y" se vaya incrementando en 1 cada vuelta, es decir, si "y" llega a ser mayor que "height", no entrará al siguiente bloque de código. 
            {
                GameObject go = GameObject.Instantiate(puzzlePiece) as GameObject;      //Creo una variable llamada "go" que guardará la instacia de la "puzzlePiece" (Variable anteriormente creada).
                Vector3 position = new Vector3(x, y, 0);                                //Creo una variable de tipo "Vector3" en donde estaré guardando una posición que dependerá de lo que valgan las variables "x" y "y" en cada repetición de los bucles "for".
                go.transform.position = position;                                       //A ese objeto guardado en "go" le asigno una posición en el mundo con la variable "Vector3" creada en la línea anterior.
                grid[x, y] = go;                                                        //Por último en esta función "Start" voy asignando la instancia "go" a la matriz, y la posición o lugar que ocupa en dicha matriz la define los valores que tengan "x" y "y" en cada vuelta por ambos bucles.
            }
        }
    }

    void Update()                                                                   //En la función "Update" escribo el bloque de código que se debe ir actualizando en cada momento, y es el siguiente.
    {
        if (loopOutput == true && timeLeft > 0)                                     //Creo una condición que dice: Si "loopOutput" (anteriormente creada) es "true" (Verdadera) y "timeLeft" es mayor a cero, si esto se cumple derberá entrar al siguiente bloque de código.
        {
            timeLeft -= Time.deltaTime;                                             //A la variable "timeLeft" le voy restando "Time.deltaTime" cada que esta función se ejecute, al hacerse esto, la variable cada vez irá restando valor y parecerá como un reloj en cuenta regresiva, ya que va restando segundos.
            if (timeLeft < 0)                                                       //En este condicional debo verificar si "timeLeft" llegó a cero, para que pueda entrar a lo siguiente.
            { 
                personalRule();                                                     //"timeLeft" funciona como un cronómetro, una vez se cumpla el tiempo asignado llamará la función "PersonalRule".
                loopOutput = false;                                                 //"loopOutput" se vuelve "false" (falso) porque, al "Update" al ser llamado cada frame, el condicional se repetiría durante todo el juego, por lo tanto puede presentar errores, solo queremos que se llame una vez completado el tiempo, así que por esa razon la variable booleana se convierte en falso, para que la condición ya no se cumpla.
            }
        }

        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);    //"mPosition" guardará la posición del mouse en el mundo, y así podremos saber en qué coordenadas esta el mouse para poder implementarlo.
        DefiningTurn(mPosition); /**/ UpdatePickedPiece(mPosition);                 //En esta línea estamos llamando dos funciones que reciben como argumento la posicion del mouse, los argumentos se entregan en los paréntesis, por esa razón ponemos la variable "mPosition" entre estos.
    }

    void UpdatePickedPiece(Vector3 position)                                                                                                //Esta función se encargará de pintar la "puzzlePiece" que tenga el mouse debajo para ayudar a ver cual es el que estamos señalando. 
    {
        int x = (int)(position.x + 0.5f); /**/ int y = (int)(position.y + 0.5f);                                                            //Creamos una variable "x" y una "y" para manejar las coordenadas que recibimos como argumentos, aparte de esto le estamos diciendo que, en el caso de "x", va a ser igual al argumento que recibe la función, es decir "position" pero accedemos a la x de esa posición, y la convertimos a entero o "int". El sumar 0.5 hace que se se pueda redondear al número más cercano del que se recibio como argumento. lo mismo aplica para "y".
                                                                                                                                            //Ejemplo de la línea anterior: Como el argumento que recibe la función es de tipo "Vector3", supongamos que "position" = (3.3, 4.7, 0). Vemos que "x" es de tipo "float" por sus decimales, al igual que "y", por lo tanto el (int) que esta después del igual lo que hace es convertir el número que haya en el siguiente parentesis, en caso de "x" es "position.x" osea 3.3 en este caso. El programa redondea hacia abajo, es decir que quedará en 3, pero lo que realmente queremos es que redondee al número más cercano, en caso de "position.y", que vale 4.7 no queremos que redondee a 4 si no a 5, para eso es la suma de 0.5.
        for (int k = 0; k < width; k++)                                                                                                     //La condición de este ciclo dice: para el entero "k" que se inicializa en 0, siempre que sea menor a "width", se le sume de a 1 en cada vuelta.
        {
            for (int j = 0; j < height; j++)                                                                                                //Este blucle es el mimso que el anterior solo que cambiamos "i" por "j" y "width" por "height".
            {
                GameObject go = grid[k, j];                                                                                                 //Los bucles se hacen para recorrer toda la matriz "grid[,]" y aplicar el a cada posición de dicha matriz las siguientes líneas dentro de los ciclos.

                if (go.GetComponent<Renderer>().material.color != playerOne && go.GetComponent<Renderer>().material.color != playerTwo)     //En este condicional verificamos si la posición específica de la matriz dada por [k,j] es diferente al color almacenado en "playerOne" (Rojo) y también si es diferente a "playerTwo" (Azul), en ese caso, entramos al siguiente bloque de código.
                {
                    go.GetComponent<Renderer>().material.color = standartColor;                                                             //Si lo anterior se cumple, es porque las esferas que estamos verificando no son ni rojas ni azules, por lo tanto, para esas que tienen color distinto, se pintan de "standartColor" (blanco).
                }
            }
        }

        if (x >= 0 && y >= 0 && x < width && y < height)                                                                                    //Con este "if" simplemente verificamos que "x" este dentro del rango de la matriz al igual que "y", como sabemos estas variables son las coordenadas del mouse en el mundo, así que la condición se hace para que lo que sigue solo se ejecute cuando el mouse esta señalando dentro de la matriz, así nos aseguramos de que si el mouse esta por fuera de la cuadrícula no presente ningún error.
        {
            GameObject go = grid[x, y];                                                                                                     //Teniendo en cuenta la línea anterior, si el mouse esta dentro, el objeto que este en el "grid[x,y]", es decir, las posiciones del mouse, lo tomamos en cuenta para la siguiente línea;

            if (go.GetComponent<Renderer>().material.color != playerOne && go.GetComponent<Renderer>().material.color != playerTwo)         //Después de tener la variable "go" que vemos que es igual a "grid[x,y]", entramos al condicional y comparamos si el color de ese objeto tomado es diferente a "playerOne" y "playerTwo", si se cumple entramos a lo siguiente.
            {
                go.GetComponent<Renderer>().material.color = signalColor;                                                                   //Ese "puzzlePiece" se pintará de "signalColor" (Negro), y esto es lo que nos sirve para ver en cual objeto tiene el mouse señalando.
            }
        }
    }

    void DefiningTurn(Vector3 position)                                                                                 //Esta función llamada PaintGrid recibe un "Vector3" como argumento.
    {
        int x = (int)(position.x + 0.5f); /**/ int y = (int)(position.y + 0.5f);                                        //Hasta aquí esta haciendo lo mismo que la función pasada, sumarle 0.5 a las coordenadas que reciba para redondearlas de la forma que queremos, simplemente que "x" y "y" ya no serán utilizadas igual que en la función anterior. 

        if (playCounter % 2 == 0)                                                                                       //En el condicional dice: Si el sobrante de la división entre "playCounter" (Variable declara al inicio) y 2 es igual a 0, entre al condicional, esto definirá los turnos, es decir, si es 0 el turno es del jugador 1, si no pasaríamos al "else" de más abajo 
        {
            if (x >= 0 && y >= 0 && x < width && y < height && youCanPaint == true && Input.GetButtonDown("Fire1"))     //La primera parte del condicional es la misma de antes que verifica que este dentro del rango, la segunda parte dice que si "youCanPaint" es igual a "true", esto es porque habrá ocasiones en donde no se podrá pintar, en esos momentos la variable estará en falso y no entrará, pero en este momento la variable es verdadera así que pasamos a la última parte del condicional que dice, si se ha presionado "Fire1", es decir, el clic izquierdo del ratón, si todo esto se cumple, pasamos a lo siguiente.
            {
                GameObject selectedObject = grid[x, y];                                                                 //El objeto que se le dio clic se guarda en la variable "selectObject" para luego darla como argumento.
                PaintGrid(selectedObject, playerOne, x, y);                                                             //Ahora llamamos la función que pinta dicha esfera y le damos como argumentos el objeto seleccionado ("selectedObject"), el color que debe implementar ("playerOne" en este primer caso) y las coordenadas en "x" y "y". 
            }
        }
        else                                                                                                            //Este "else" es en caso que no se cumpla la condición anterior, realmente el código que sigue es el mismo que el del "if" anterior, simplemente cambiano el color como argumento ya que este "else" se aplica para el jugador dos, mientras que el "if" es para el jugador uno, así que en vez de "playerOne" como segundo argumento se pone "playerTwo".
        {
            if (x >= 0 && y >= 0 && x < width && y < height && youCanPaint == true && Input.GetButtonDown("Fire1"))     //Misma condición.
            {
                GameObject selectedObject = grid[x, y];                                                                 //Exactamente lo mismo que en la condición anterior.
                PaintGrid(selectedObject, playerTwo, x, y);                                                             //Cambiamos el segundo argumento.
            }
        }
    }

    void PaintGrid(GameObject selected, Color colors, int x, int y)                                                     //LLegamos a la función "PaintGrid" llamada anteriormente y se encargará de pintar los objetos
    {
        if (selected.GetComponent<Renderer>().material.color == signalColor)                                            //Esta condición nos dice que si el objeto "select", que es el objeto seleccionado que se recibió como argumento, es igual a "signalColor", podremos pasar a la siguiente línea.
        {
            selected.GetComponent<Renderer>().material.SetColor("_Color", colors);                                      //Aquí simplemente a ese objeto seleccionado le damos como color "colors", variable obtenida como argumento de la función anterior, básicamente pintamos el objeto con su color correspondiente.
            Color current = selected.GetComponent<Renderer>().material.color;                                           //Luego creamos una variable de tipo "Color" para guardar ese color que acabamos de dar, es decir, que si el color de "selected" es igual a rojo, este color se almacenará en "current".
            loopOutput = true;                                                                                          //"loopOutput" se activa, es decir, se pone verdadera en el momento en el que se pinta la primera esfera, para así empezar a disminuir "timeLeft" en el "Update".
            playCounter++;                                                                                              //Aumentamos en uno "playCounter" que es el número de jugadas que se han hecho, al pintar un objeto quiere decir que ya se hizo una jugada, así que por eso se aumenta.
            noWinnerCount++;                                                                                            //"noWinner" se empieza a sumar cada que se pinta de color y es el contador que verifica las jugadas para el caso de empate.
            counterColorH = 0; /**/ counterColorV = 0; /**/ counterColorRD = 0; /**/ counterColorLD = 0;                //Todos los contadores que sirven para verificar que hayan cuatro del mismo color seguidas (que son usados en las siguientes funciones), se definen en 0 en esta línea siempre antes de llamar las funciones para evitar algún tipo de acumulación y por ende un error.
            HorizontalVerification(y, current); /**/ VerticalVerification(x, current);                                  //Llamamos las funciones que verifican de forma horizontal y vertical.
            RightDiagonalCheck(x, y, current); /**/ LeftDiagonalCheck(x, y, current);                                   //Luego Llamamos las que verifican las diagonales derechas e izquierdas, y tanto estas dos como las de la línea anterior reciben los mismos argumentos, los dos primeros argumentos son de tipo "int" y el tercero es de tipo "Color", así que los entregamos.
            DisappearAppear(playCounter);
            NoWinner(noWinnerCount);                                                                                    //Por último en esta función "paintGrid", llamamos "NoWinner" y damos como argumento el contador de jugadas.
        }
    }
    
    void HorizontalVerification(int j,  Color colors)                               //Ahora entramos en la función que verifica las filas, es decir, la forma horizontal, para aclarar: "k" cumplirá la función de "x" y "j" de "y" como coordenadas.
    {
        for (int k = 0; k < width; k++)                                             //Creamos un blucle en el que "k" empezara siendo 0 y se irá sumando de a una unidad por vuelta hasta que se salga de la condición, es decir, cuando sea mayor a "width" se saldrá del "for".
        {
            if (k >= 0 && j >= 0 && k < width && j < height)                        //Volvemos a la condición que verifique si está dentro del rango de la matriz.
            {
                Color next = grid[k, j].GetComponent<Renderer>().material.color;    //Creamos una variable de tipo "Color" que almacenará el color de "grid[k,j]". Como vemos, "j" en ningún momento cambia de valor en esta función, esto es porque para verificar horizontalmente el que debe ir cambiando de posición es "x", en este caso recordemos que es "k".
                                                                                    //Ejemplo: Supongamos que como argumento de enteros se recibe 3 para "k" y 4 para "j", si nos imaginamos un plano cartesiano, "j" (es decir "y") seguirá siendo 4, mientras que "k" (es decir "x"), empezará desde cero e irá aumentando. En resumen, en la primera vuelta las coordenadas serán (0,4) y se le irá sumando uno así: (1,4)(2,4)(3,4)(4,4) y asi sucesivamente veremos que se recorrer toda esa fila.
                if (colors == next)                                                 //Con este "if" verificamos si el color que dimos como argumento es igual a el que acabamos de crear, en ese caso entramos a lo siguiente.
                {
                    counterColorH++;                                                //La variable "counterColorH" aumenta en uno.

                    if (counterColorH == 4 && colors == playerOne)                  //Este condicional es el que nos avisa de la victoria, si "counterColorH" llega a ser 4 es porque hay cuatro seguidas del mismo color, pero aparte verificamos qué color es el que esta seguido.
                    {
                        StartCoroutine("WaitTime", colors);                         //Si lo anterior se cumple entra a llamar la corrutina que entre comillas va el nombre de la corrutina y luego, ya que este debe recibir un argumento de tipo "Color", se entrega separándolo del nombre por una coma.
                        break;                                                      //Después simplemente con la palabra "break", rompemos el bucle porque así se evitan fallos y además no tiene sentido que se siga ejecutando el "for" si ya se encontro un ganador.
                    }
                    else if(counterColorH == 4 && colors == playerTwo)              //Este "else" lo único que cambia con respecto al "if" anterior es el color con el que compara, ya no es "playerOne" sino "playerTwo".
                    {
                        StartCoroutine("WaitTime", colors);                         //Llamamos la corrutina con un color distinto al anterior.
                        break;                                                      //Y rompemos el ciclo.
                    }
                }
                else                                                                //Este "else" es la alternativa al "if" que compara "colors" con "next", en caso de que ambos colores no sean iguales.
                {
                    counterColorH = 0;                                              //El contador vuelve a 0 para que no se acumule.
                }
            }
        }
    }

    void VerticalVerification(int k, Color colors)                                  //Ahora entramos en la función que verifica las columnas, es decir, la forma vertical, para aclarar: "k" cumplirá la función de "x" y "j" de "y" como coordenadas.
    {
        for (int j = 0; j < height; j++)                                            //Creamos un blucle en el que "j" empezara siendo 0 y se irá sumando de a una unidad por vuelta hasta que se salga de la condición, es decir, cuando sea mayor a "height" se saldrá del "for".
        {
            if (k >= 0 && j >= 0 && k < width && j < height)                        //Volvemos a la condición que verifique si está dentro del rango de la matriz.
            {
                Color next = grid[k, j].GetComponent<Renderer>().material.color;    //Creamos una variable de tipo "Color" que almacenará el color de "grid[k,j]" como vemos "k" en ningún momento cambia de valor en esta función, esto es porque para verificar verticalmente el que debe ir cambiando de posición es "y", en este caso recordemos que es "j".
                                                                                    //Ejemplo: Supongamos que como argumento de enteros se recibe 8 para "k" y 5 para "j", si nos imaginamos un plano cartesiano, "k" (es decir "x") seguirá siendo 8, mientras que "j" (es decir "y"), empezará desde cero e irá aumentando, en resumen, en la primera vuelta las coordenadas serán (8,0) y se le irá sumando uno así: (8,1)(8,2)(8,3)(8,4) y asi sucesivamente veremos que se recorrer toda esa columna.
                if (colors == next)                                                 //Con este "if" verificamos si el color que dimos como argumento es igual a el que acabamos de crear, en ese caso entramos a lo siguiente.
                {
                    counterColorV++;                                                //La variable "counterColorV" aumenta en uno.

                    if (counterColorV == 4 && colors == playerOne)                  //Este condicional es el que nos avisa de la victoria, si "counterColorV" llega a ser 4 es porque hay cuatro seguidas del mismo color, pero aparte verificamos qué color es el que esta seguido.
                    {
                        StartCoroutine("WaitTime", colors);                         //Si lo anterior se cumple entra a llamar la corrutina que entre comillas va el nombre de la corrutina y luego, ya que este debe recibir un argumento de tipo "Color", se entrega separándolo del nombre por una coma.
                        break;                                                      //Después simplemente con la palabra "break", rompemos el bucle porque así se evitan fallos y además no tiene sentido que se siga ejecutando el "for" si ya se encontro un ganador.
                    }
                    else if (counterColorV == 4 && colors == playerTwo)             //Este "else" lo único que cambia con respecto al "if" anterior es el color con el que compara, ya no es "playerOne" sino "playerTwo".
                    {
                        StartCoroutine("WaitTime", colors);                         //Llamamos la corrutina con un color distinto al anterior.
                        break;                                                      //Y rompemos el ciclo.
                    }
                }
                else                                                                //Este "else" es la alternativa al "if" que compara "colors" con "next", en caso de que ambos colores no sean iguales.
                {
                    counterColorV = 0;                                              //El contador vuelve a 0 para que no se acumule.
                }
            }
        }
    }

    void RightDiagonalCheck(int k, int j, Color colors)                                     //Ahora entramos en la función que verifica las diagonales que van hacia la derecha, para aclarar: "k" cumplirá la función de "x" y "j" de "y" como coordenadas.
    {
        for (; k >= 0; k--)                                                                 //La funcionalidad de este "for" será encontrar el valor minimo de "k" y "j", este bucle le irá restando una unidad a "k" y a "j", hasta que uno de los dos llegue a cero, que de hecho es el siguiente condicional
        {
            if (k == 0 || j == 0)                                                           //Como decía, el bucle se ejecutará y se ejecutará hasta que este condicional sea verdadero, como vemos, "k" se va restando en el mismo "for", mientras que "j" se resta al final con (j--), entonces el condicional nos dice que cuando uno de los dos llegue a cero se puede accerder al código dentro de este.
            {                                                                               //Ejemplo: si como argumento nos llega "k" igual a 5 y "j" igual a 2, las coordenadas en la primera vuelta serán esas (5,2), en este momento no podrá entrar al "if". En la segunda vuelta las coordenadas serán (4,1) y en la tercera (3,0), en este punto ahora si podrá accerder al condicional.
                for (; k < width; k++)                                                      //Una vez entrado, se crea el "for" que hace todo lo contrario que el anterior, este ira sumando de a 1 a "k" y a "j" para así descubrir todos los valores de la diagonal específica.
                {
                    if (k >= 0 && j >= 0 && k < width && j < height)                        //Volvemos a la condición que verifica si está dentro del rango de la matriz.
                    {
                        Color next = grid[k, j].GetComponent<Renderer>().material.color;    //Creamos una variable de tipo "Color" que almacenará el color de "grid[k,j]" dependiendo del valor de estos.

                        if (colors == next)                                                 //Con este "if" verificamos si el color que dimos como argumento es igual a el que acabamos de crear, en ese caso entramos a lo siguiente.
                        {
                            counterColorRD++;                                               //La variable "counterColorRD" aumenta en uno.

                            if (counterColorRD == 4 && colors == playerOne)                 //Este condicional es el que nos avisa de la victoria, si "counterColorRD" llega a ser 4 es porque hay cuatro seguidas del mismo color, pero aparte verificamos qué color es el que esta seguido.
                            {
                                StartCoroutine("WaitTime", colors);                         //Si lo anterior se cumple entra a llamar la corrutina que entre comillas va el nombre de la corrutina y luego, ya que este debe recibir un argumento de tipo "Color", se entrega separandolo del nombre por una coma.
                                break;                                                      //Despues simplemente con la palabra "break", rompemos el bucle porque así se evitan fallos y además no tiene sentido que se siga ejecutando el "for" si ya se encontro un ganador.
                            }
                            else if (counterColorRD == 4 && colors == playerTwo)            //Este "else" lo único que cambia con respecto al "if" anterior es el color con el que compara, ya no es "playerOne" sino "playerTwo".
                            {
                                StartCoroutine("WaitTime", colors);                         //Llamamos la corrutina con un color distinto al anterior.
                                break;                                                      //Y rompemos el ciclo.
                            }
                        }
                        else                                                                //Este "else" es la alternativa al "if" que compara "colors" con "next", en caso de que ambos colores no sean iguales.
                        {
                            counterColorRD = 0;                                             //El contador vuelve a 0 para que no se acumule.
                        }
                    }
                    j++;                                                                    //Este es la "j" que irá aumentando con el segundo "for".
                }
                break;                                                                      //El "break" está fuera del segundo "for" así que a este no lo va a afectar, este "break" romperá el primero.
            }
            j--;                                                                            //"j" disminuirá su valor con el primer "for" para encontrar su mínimo.
        }     
    }

    void LeftDiagonalCheck(int k, int j, Color colors)                                      //Ahora entramos en la función que verifica las diagonales que van hacia la izquierda, para aclarar: "k" cumplirá la función de "x" y "j" de "y" como coordenadas.
    {
        for (; k < width; k++)                                                              //La funcionalidad de este "for" será encontrar el valor máximo de "k" y el minimo de "j", este bucle le irá sumando una unidad a "k" y restando a "j", hasta que se cumpla el siguiente condicional.
        {
            if (k == (width - 1) || j == 0)                                                 //El máximo valor para "k" será "width" - 1, en este caso 9, y el mínimo de "j" será cero, cuando uno de estos dos casos se cumpla entrará al siguiente bloque de código.
            {
                for (; k >= 0; k--)                                                         //Utilizando la lógica, una vez "k" haya llegado a su valor máximo habrá que ir restando para que los valores vayan hacia la izquierda.
                {
                    if (k >= 0 && j >= 0 && k < width && j < height)                        //Volvemos a la condición que verifique si está dentro del rango de la matriz.
                    {
                        Color next = grid[k, j].GetComponent<Renderer>().material.color;    //Creamos una variable de tipo "Color" que almacenará el color de "grid[k,j]" dependiendo del valor de estos.

                        if (colors == next)                                                 //Con este "if" verificamos si el color que dimos como argumento es igual a el que acabamos de crear, en ese caso entramos a lo siguiente.
                        {
                            counterColorLD++;                                               //La variable "counterColorLD" aumenta en uno.

                            if (counterColorLD == 4 && colors == playerOne)                 //Este condicional es el que nos avisa de la victoria, si "counterColorLD" llega a ser 4 es porque hay cuatro seguidas del mismo color, pero aparte verificamos qué color es el que esta seguido.
                            {
                                StartCoroutine("WaitTime", colors);                         //Si lo anterior se cumple entra a llamar la corrutina que entre comillas va el nombre de la corrutina y luego, ya que este debe recibir un argumento de tipo "Color", se entrega separandolo del nombre por una coma.
                                break;                                                      //Despues simplemente con la palabra "break", rompemos el bucle porque así se evitan fallos y además no tiene sentido que se siga ejecutando el "for" si ya se encontro un ganador.
                            }
                            else if (counterColorLD == 4 && colors == playerTwo)            //Este "else" lo único que cambia con respecto al "if" anterior es el color con el que compara, ya no es "playerOne" sino "playerTwo".
                            {
                                StartCoroutine("WaitTime", colors);                         //Llamamos la corrutina con un color distinto al anterior.
                                break;                                                      //Y rompemos el ciclo.
                            }
                        }
                        else                                                                //Este "else" es la alternativa al "if" que compara "colors" con "next", en caso de que ambos colores no sean iguales.
                        {
                            counterColorLD = 0;                                             //El contador vuelve a 0.
                        }
                    }
                    j++;                                                                    //Este es la "j" que irá aumentando con el segundo "for".
                }
                break;                                                                      //El "break" está fuera del segundo "for" así que a este no lo va a afectar, este "break" romperá el primero.
            }
            j--;                                                                            //"j" disminuirá su valor con el primer "for" para encontrar su valor mínimo.
        }
    }

    void ResetGrid()                                                                    //Esta es la función que nos limpiará el tablero en caso de que haya un ganador, o empate, para aclarar: "k" cumplirá la función de "x" y "j" de "y" como coordenadas.
    {
        for (int k = 0; k < width; k++)                                                 //Creamos un "for" que recorrerá todos los valores en "k", empezando desde cero hasta que sea menor a "width".
        {
            for (int j = 0; j < height; j++)                                            //Creamos un "for" que recorrerá todos los valores en "j", empezando desde cero hasta que sea menor a "height".
            {
                grid[k, j].GetComponent<Renderer>().material.color = standartColor;     //Cada que llega a una esfera la pinta con "standartColor".
            }
        }
    }

    void NoWinner(int count)                                //En esta función se recibe un entero como argumento, y este es el contador de jugadas, es decir, cuando se llamó anteriormente se dio como argumento la variable "playCounter".
    {
        if(count == 100)                                    //Simpremente si el argumento "count" llega a ser 100 significa que hay un empate.
        {
            StartCoroutine("WaitTime", standartColor);      //Por ende se inicia la corrutina, con su respectivo argumento.
        }
    }

    IEnumerator WaitTime(Color colors)          //Llegamos a la corrutina con argumento de tipo "Color".
    {
        noWinnerCount = 0;                      //El contador para el empate vuelve a cero una vez alguien gana.
        loopOutput = false;                     //La variable "loopOutPut" que se utiliza para entrar en el condicional del update se vuelve falsa, para volverla a activar una vez se pinte un objeto.
        youCanPaint = false;                    //La variable boolena utilizada para poder entrar a los turnos se vuelve falsa, para que cuando alguien gane, no se pueda pintar más.
        ScorePoints(colors);                    //Llamamos la función "ScorePoints" con su respectivo argumento.
        yield return new WaitForSeconds(3);     //Luego decimos que hay que esperar 3 segundos para continuar.
        ResetGrid();                            //Después de esos tres segundos llamamos la función que resetea el tablero.
        youCanPaint = true;                     //"youCanPaint" será verdadera para poder volver a pintar.
        timeLeft = Random.Range(20f, 30f);      //Y se vuelve a inicializar "timeLeft" para que tome un valor distinto cada que se resetea el tablero.
    }

    IEnumerator TimeForText()                   //Esta corrutina solo manejará la activación de "ChangeColor" y "WinText"
    {
        yield return new WaitForSeconds(3f);    //Decimos que espere tres segundos.
        changeColor.SetActive(false);           //Desactiva "changeColor"
    }

    void ScorePoints(Color winner)                  //Entramos en la función que permite ver los puntajes en la pantalla.
    {
        if (winner == playerOne)                    //Según el color que reciba como argumento verificamos si es igual al "playerOne" (Rojo) o a "playerTwo" (Azul).
        {
            redPoints++;                            //En caso que sea rojo aumentamos en uno a la variable "redPoints" que es el contador del color rojo.
            redText.text = redPoints.ToString();    //Y cambiamos el texto de la variable "redText" por el contador.
        }
        else if (winner == playerTwo)               //Cambiamos la comparación para este "if".
        {
            bluePoints++;                           //Aumentamos "bluePoints".
            blueText.text = bluePoints.ToString();  //Cambiamos el texto del jugador azul.
        }
    }

    void personalRule()                                                                         //Llegamos a la función que tiene la regla personal que se llama desde el condicional del "Update" y consiste en después de que se complete "timeLeft" se cambian los colores del tablero, es decir, todos los que estaban de rojo pasarán a ser azules, y al contrario.
    {
        changeColor.SetActive(true);                                                            //Se activa el "GameObjec" que muestra el cambio de color.
        for (int k = 0; k < width; k++)                                                         //Aplico de nuevo el for que recorre todos los valores de "k".
        {
            for (int j = 0; j < height; j++)                                                    //Aplico de nuevo el for que recorre todos los valores de "j".
            {
                if (grid[k, j].GetComponent<Renderer>().material.color == playerOne)            //Este condicional verifica si el "grid[k,j]" es rojo.
                {
                    grid[k, j].GetComponent<Renderer>().material.color = playerTwo;             //Pasara a pintarlos de azul.
                }
                else if (grid[k, j].GetComponent<Renderer>().material.color == playerTwo)        //Este condicional verifica si el "grid[k,j]" es azul.
                {
                    grid[k, j].GetComponent<Renderer>().material.color = playerOne;             //Pasará a pintarlos de rojo.
                }
            }
        }
        StartCoroutine("TimeForText");                                                          //Llamamos la corrutina que maneja el texto.
    }

    void DisappearAppear(int turn)                                  //Esta función hará aparecer y desaparecer el texto de cada jugador por medio de una animación, para hacer dar cuenta de quien es el turno.
    {
        if (turn % 2 == 0)                                          //En este "if" verifico de quien es el turno, recordemos este if de la función ""DefiningTurn", entonces si es turno del primer jugador se hace lo siguiente.
        {
            redTurn.GetComponent<Animator>().enabled = true;        //La animación del rojo se activa para que empiece a desaparecer y a aparecer.
            blueTurn.GetComponent<Animator>().enabled = false;      //La animacion del azul se desactiva porque ya no es su turno.
            blueTurn.GetComponent<Text>().enabled = true;           //Y como la animación lo que hace aparecer y desaparecer el texto que tiene el "GameObject", entonces decimos que el componente de texto del azul se active, juntando esta línea con la anterior, el texto de quien no es el turno, quedará activado y sin animación.
        }
        else                                                        //Este "else" es para saber que es el turno del segundo jugador y cumplirá lo contrario del "if" anterior.
        {
            redTurn.GetComponent<Animator>().enabled = false;       //El texto rojo se deja de animar.
            blueTurn.GetComponent<Animator>().enabled = true;       //El texto azul empieza a aparecer y a desaparecer.
            redTurn.GetComponent<Text>().enabled = true;            //Por último, el texto rojo aparece y queda firme hasta el siguiente turno.
        }
    }
}