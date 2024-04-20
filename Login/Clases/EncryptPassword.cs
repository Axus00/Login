using System.Text;

namespace Login.Clases;

public static class EncryptPassword
{
    //Encrypt password
    public static string CifrarPassword(string password, int movimiento)
    {
        string letters = "abcdefghijklmnñopqrstuvwxyz";
        string passwordCifrado = "";

        foreach (char caracter in password)
        {
            if (char.IsLetter(caracter))
            {
                int posicionCaracter = letters.IndexOf(char.ToLower(caracter));
                int nuevaPosicion = (posicionCaracter + movimiento) % letters.Length;
                char otroCaracter = letters[nuevaPosicion];
                passwordCifrado += char.IsUpper(caracter) ? char.ToUpper(otroCaracter) : otroCaracter;
            }
            else
            {
                passwordCifrado += caracter;
            }

            if (char.IsNumber(caracter))
            {
                int posicionCaracter = letters.IndexOf(char.ToLower(caracter));
                int nuevaPosicion = (posicionCaracter + movimiento) % letters.Length;
                char otroCaracter = letters[nuevaPosicion];
                passwordCifrado += char.IsUpper(caracter) ? char.ToUpper(otroCaracter) : otroCaracter;
            }
            else
            {
                passwordCifrado += caracter;
            }
        }

        return passwordCifrado;
    }
    
    //desencrypt password
    public static string DesencriptarPassword(string password, int moviemiento)
    {
        string letters = "abcdefghijklmnñopqrstuvwxyz";
        string passwordDescifrado = "";

        foreach (char caracter in password)
        {
            if (char.IsLetter(caracter))
            {
                int posicionCaracter = letters.IndexOf(char.ToLower(caracter));
                int nuevaPosicion = (posicionCaracter - moviemiento + letters.Length) % letters.Length;
                char otroCaracter = letters[nuevaPosicion];
                passwordDescifrado += char.IsUpper(caracter) ? char.ToUpper(otroCaracter) : otroCaracter;
            }
            else
            {
                passwordDescifrado += caracter;
            }

            if (char.IsNumber(caracter))
            {
                int posicionCaracter = letters.IndexOf(char.ToLower(caracter));
                int nuevaPosicion = (posicionCaracter - moviemiento + letters.Length) % letters.Length;
                char otroCaracter = letters[nuevaPosicion];
                passwordDescifrado += char.IsUpper(caracter) ? char.ToUpper(otroCaracter) : otroCaracter;
            }
            else
            {
                passwordDescifrado += caracter;
            }
        }

        return passwordDescifrado;
    }
}