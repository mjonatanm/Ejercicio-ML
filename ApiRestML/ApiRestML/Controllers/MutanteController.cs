using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace ApiRestML.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/mutante")]
    public class MutanteController : ApiController
    {
        [HttpPost]
        [Route("mutant")]
        public IHttpActionResult mutant(string[] dna)
        {
            string valor = "";
            bool error = false;
            int isMutantSequence = 0;
            int i = 0;
            int cantidadVueltas = 0;

            int cantidadFilas = dna.Count();
            int cantidadColumna = dna[0].Count();

            #region Validaciones
            for (int indice = 1; indice < dna.Length; indice++)
            {
                if (cantidadColumna != dna[indice].Length)
                {
                    error = true;
                    break;
                }
            }

            if ((cantidadFilas < 4 || cantidadColumna < 4) || (cantidadFilas != cantidadColumna))
            {
                error = true;
            }

            for (int incremento = 0; incremento < dna.Length; incremento++)
            {
                if (!ValidaContenido(dna[incremento]))
                {
                    error = true;
                    break;
                }
            }

            #endregion

            if (!error)
            {
                #region Carga de la Matriz.
                string[,] matriz = cargarMatriz(dna, cantidadFilas, cantidadColumna);
                #endregion

                #region Recorrido de todas las filas Variables correctas.
                for (int fila = 0; fila < cantidadFilas; fila++)
                {
                    while (cantidadColumna - cantidadVueltas > 3)
                    {
                        for (int columna = cantidadVueltas; columna < cantidadColumna; columna++)
                        {
                            if (valor.Length < 4)
                            {
                                valor = valor + matriz[fila, columna];
                            }
                        }

                        if (checkMutant(valor))
                        {
                            isMutantSequence++;
                        }

                        if (isMutantSequence == 2)
                        {
                            break;
                        }

                        valor = "";
                        cantidadVueltas++;
                    }

                    if (isMutantSequence == 2)
                    {
                        break;
                    }

                    valor = "";
                    cantidadVueltas = 0;
                }
                #endregion

                #region Recorrido de todas las columnas Variables correctas.
                if (isMutantSequence < 2)
                {
                    valor = "";
                    cantidadVueltas = 0;
                    for (int columna = 0; columna < cantidadColumna; columna++)
                    {
                        while (cantidadColumna - cantidadVueltas > 3)
                        {
                            for (int fila = cantidadVueltas; fila < cantidadFilas; fila++)
                            {
                                if (valor.Length < 4)
                                {
                                    valor = valor + matriz[fila, columna];
                                }
                            }

                            if (checkMutant(valor))
                            {
                                isMutantSequence++;
                            }

                            if (isMutantSequence == 2)
                            {
                                break;
                            }

                            valor = "";
                            cantidadVueltas++;
                        }

                        if (isMutantSequence == 2)
                        {
                            break;
                        }

                        valor = "";
                        cantidadVueltas = 0;
                    }
                }
                #endregion

                #region Recorrido de la diagonal principal. Variables correctas.
                if (isMutantSequence < 2)
                {
                    valor = "";
                    cantidadVueltas = 0;
                    while (cantidadColumna - cantidadVueltas > 3)
                    {
                        for (int columna = 0; columna < cantidadColumna; columna++)
                        {
                            for (int fila = cantidadVueltas; fila < cantidadFilas; fila++)
                            {
                                if (fila == columna)
                                {
                                    if (valor.Length < 4)
                                    {
                                        valor = valor + matriz[fila, columna];
                                    }
                                }
                            }
                        }

                        if (checkMutant(valor))
                        {
                            isMutantSequence++;
                        }

                        if (isMutantSequence == 2)
                        {
                            break;
                        }
                        valor = "";
                        cantidadVueltas++;
                    }
                }
                #endregion

                #region Recorro las diagones de centro + 1 a la derecha. Variables correctas.
                if (isMutantSequence < 2)
                {
                    valor = "";
                    cantidadVueltas = 0;
                    while (cantidadColumna - cantidadVueltas > 3)
                    {
                        for (int col = 0; col < cantidadColumna; col++)
                        {
                            for (int fila = col; fila < cantidadFilas; fila++)
                            {
                                if (!(cantidadColumna - col <= 4 + cantidadVueltas))
                                {
                                    if (valor.Length < 4)
                                    {
                                        valor = valor + matriz[fila, fila + 1 + cantidadVueltas];
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (valor == "")
                            {
                                break;
                            }
                            else
                            {
                                if (checkMutant(valor))
                                {
                                    isMutantSequence++;
                                }

                                if (isMutantSequence == 2)
                                {
                                    break;
                                }
                                valor = "";
                            }
                        }
                        if (isMutantSequence == 2)
                        {
                            break;
                        }
                        cantidadVueltas++;
                    }
                }
                #endregion

                #region Recorro las diagones de centro - 1 a la derecha. Variables correctas.
                if (isMutantSequence < 2)
                {
                    valor = "";
                    cantidadVueltas = 0;
                    while (cantidadColumna - cantidadVueltas > 3)
                    {
                        for (int fila = 0; fila < cantidadFilas; fila++)
                        {
                            for (int col = fila; col < cantidadColumna; col++)
                            {
                                if (!(cantidadColumna - fila <= 4 + cantidadVueltas))
                                {
                                    if (valor.Length < 4)
                                    {
                                        valor = valor + matriz[col + 1 + cantidadVueltas, col];
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (valor == "")
                            {
                                break;
                            }
                            else
                            {
                                if (checkMutant(valor))
                                {
                                    isMutantSequence++;
                                }

                                if (isMutantSequence == 2)
                                {
                                    break;
                                }
                                valor = "";
                            }
                        }
                        if (isMutantSequence == 2)
                        {
                            break;
                        }
                        cantidadVueltas++;
                    }
                }
                #endregion

                #region Recorro diagonal inversa. Variables correctas.
                if (isMutantSequence < 2)
                {
                    valor = "";
                    cantidadVueltas = 0;
                    while (cantidadColumna - cantidadVueltas > 3)
                    {
                        for (int fila = cantidadVueltas; fila < cantidadFilas; fila++)
                        {
                            for (int col = 0; col < cantidadColumna; col++)
                            {
                                if (fila + col == cantidadColumna - 1)
                                {
                                    if (valor.Length < 4)
                                    {
                                        valor = valor + matriz[fila, col];
                                    }
                                }
                            }
                        }

                        if (checkMutant(valor))
                        {
                            isMutantSequence++;
                        }

                        if (isMutantSequence == 2)
                        {
                            break;
                        }
                        valor = "";
                        cantidadVueltas++;
                    }
                }
                #endregion

                #region Recorro las diagonales inversa de centro - 1 a la derecha. Variables correctas.
                if (isMutantSequence < 2)
                {
                    valor = "";
                    cantidadVueltas = 0;
                    while (cantidadColumna - 1 - cantidadVueltas > 3)
                    {
                        for (int col = cantidadColumna - 1; col > 2; col--)
                        {
                            for (int fila = 1; fila < cantidadFilas; fila++)
                            {
                                if (col > 3 + cantidadVueltas)
                                {
                                    if (!(cantidadFilas - 1 - fila + cantidadVueltas <= 1 + cantidadVueltas))
                                    {
                                        if (valor.Length < 4)
                                        {
                                            valor = valor + matriz[fila + i, col - fila + 1];
                                        }
                                    }
                                }
                            }

                            if (valor == "")
                            {
                                break;
                            }
                            else
                            {
                                if (checkMutant(valor))
                                {
                                    isMutantSequence++;
                                }

                                if (isMutantSequence == 2)
                                {
                                    break;
                                }
                                i++;
                                valor = "";
                            }
                        }

                        if (isMutantSequence == 2)
                        {
                            break;
                        }

                        cantidadVueltas++;
                        i = cantidadVueltas;
                    }
                }
                #endregion

                #region Recorro las diagonales inversa de centro + 1 a la derecha. Variables correctas.
                if (isMutantSequence < 2)
                {
                    valor = "";
                    cantidadVueltas = 0;
                    i = 0;
                    while (cantidadColumna - 1 - cantidadVueltas > 3)
                    {
                        for (int col = cantidadColumna - 2 - cantidadVueltas; col > 2; col--)
                        {
                            for (int fila = 0; fila < cantidadFilas; fila++)
                            {
                                if (!(cantidadFilas - 1 - fila + cantidadVueltas <= 2 + cantidadVueltas))
                                {
                                    if (valor.Length < 4)
                                    {
                                        valor = valor + matriz[fila + i, col - fila];
                                    }
                                }
                            }

                            if (valor == "")
                            {
                                break;
                            }
                            else
                            {
                                if (checkMutant(valor))
                                {
                                    isMutantSequence++;
                                }

                                if (isMutantSequence == 2)
                                {
                                    break;
                                }
                                i++;
                                valor = "";
                            }
                        }

                        if (isMutantSequence == 2)
                        {
                            break;
                        }
                        cantidadVueltas++;
                        i = 0;
                    }
                }
                #endregion
            }

            if (!error)
            {
                if (isMutantSequence == 2)
                {
                    return new StatusCodeResult(System.Net.HttpStatusCode.OK, this);
                }
                else
                {
                    return new StatusCodeResult(System.Net.HttpStatusCode.Forbidden , this);
                }
            }
            else
            {
                return new StatusCodeResult(System.Net.HttpStatusCode.BadRequest, this);
            }
        }

        private static bool checkMutant(string valor)
        {
            int contador = 0;
            string aux_valor = valor[0].ToString();
            for (int i = 0; i < 4; i++)
            {
                if (aux_valor == valor[i].ToString())
                {
                    aux_valor = valor[i].ToString();
                    contador++;
                    if (contador == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;

        }

        private static bool ValidaContenido(string cadena)
        {
            bool errorA = false;
            bool errorT = false;
            bool errorC = false;
            bool errorG = false;
            int cantidad = 0;

            cadena = cadena.ToUpper();
            string cadenaDistinc = new String(cadena.Distinct().ToArray());
            if (cadenaDistinc.Count() > 4)
            {
                return false;
            }
            else
            {
                if (cadenaDistinc.Count() < 4)
                {
                    errorA = cadenaDistinc.Contains("A");
                    if (errorA) cantidad++;
                    errorT = cadenaDistinc.Contains("T");
                    if (errorT) cantidad++;
                    errorC = cadenaDistinc.Contains("C");
                    if (errorC) cantidad++;
                    errorG = cadenaDistinc.Contains("G");
                    if (errorG) cantidad++;

                    if (cadenaDistinc.Count() == cantidad)
                    {
                        return true;
                    }
                }
            }

            errorA = cadenaDistinc.Contains("A");
            errorT = cadenaDistinc.Contains("T");
            errorC = cadenaDistinc.Contains("C");
            errorG = cadenaDistinc.Contains("G");

            if (!errorA || !errorT || !errorC || !errorG)
            {
                return false;
            }

            return true;
        }

        #region Carga de Matriz
        private static string[,] cargarMatriz(string[] arr, int fila, int col)
        {
            string[,] matriz = new string[fila, col];

            for (int i = 0; i < fila; i++)
            {
                for (int y = 0; y < col; y++)
                {
                    matriz[i, y] = arr[i].Substring(y, 1);
                }
            }

            return matriz;
        }
        #endregion
    }
}
