using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Probleme
{
    class Plateau
    {
        // Attributs
        private De [] tabDes;
        private char[] facesSuperieures;

        // Constructeur
        public Plateau (string FileName, Random r)
        {
            tabDes = new De[16];
            facesSuperieures = new char[16];
            ReadFile(FileName, r);
            // Maintenant, on mélange les dés du tableau pour former le tableau
            int j;
            De temp;
            for (int i = 0; i< tabDes.Length; i++)
            {
                j = r.Next(tabDes.Length);
                temp = tabDes[i];
                tabDes[i] = tabDes[j];
                tabDes[j] = temp;
                facesSuperieures[i] = tabDes[i].FaceVisible;
                facesSuperieures[j] = tabDes[j].FaceVisible;
            }
            
        }

        // Propriétés
        public De [] TabDes
        {
            get { return tabDes; }
        }

        public char [] FacesSuperieures
        {
            get { return facesSuperieures; }
        }

        // Méthodes
        public void ReadFile (string FileName, Random r)
        {
            StreamReader str = null;

            string line;
            int i = 0;     
            try
            {
                str = new StreamReader(FileName);
                while ((line = str.ReadLine()) != null)
                {
                    string [] s = line.Split(';');
                    char[] lettres = new char[s.Length];
                    for (int j =0; j< s.Length; j++)  // j < 6
                    {
                        lettres[j] = char.Parse(s[j]);
                    }
                    
                    tabDes[i] = new De(lettres,r);
                    i++;
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (str != null)
                {
                    str.Close();
                }
            }
        }

        public string toString()
        {
            string rep = "";
            for (int i =0; i < 16; i++)
            {
                rep += facesSuperieures[i];
                if ((i+1) % 4 == 0)        // Si l'indice i correspond à un dé situé en bordure droite du plateau on revient à la ligne
                {
                    rep += "\n";
                }
                else
                {
                    rep += " ";
                }
            }
            return rep;
        }


        /// <summary>
        /// Recherche la première lettre du mot recherché entré en paramètre dans le plateau de jeu, 
        /// Puis fais appel à la fonction RechercheMotRecursive pour rechercher le reste des lettres du mot de sorte quelles soient adjacentes dans le plateau
        /// </summary>
        /// <param name="mot"></param>
        /// <returns></returns>
        public bool Test_PLateau(char [] mot)
        {
            // Création d'une matrice temporaire représentant le plateau de jeu
            char[,] matrice_plateau = MatriceRepresentantPlateau();

            int indice_lettre;
            bool mot_trouve = false;

            for (int i=0; i<matrice_plateau.GetLength(0) && !(mot_trouve); i++)
            {
                for (int j =0; j<matrice_plateau.GetLength(1) && !(mot_trouve); j++)
                {
                    indice_lettre = 0;
                    if (matrice_plateau[i,j] == mot[indice_lettre])
                    {
                        bool[,] matrice_cases_visitees = CasesVisitees(); // On fait l'initialisation ici si jamais on trouve plus d'une lettre potentielle dans la grille pour la première lettre du mot
                        matrice_cases_visitees[i, j] = true;             // La case contenant la première lettre du mot potentiel est indiquée comme déjà prise
                        mot_trouve = RechercheMotRecursive(i, j, mot, indice_lettre + 1, matrice_plateau, matrice_cases_visitees);
                    }
                }
            }
            return mot_trouve;
        }



        /// <summary>
        /// Recherche un mot dans le plateau de jeu lorsque la première lettre de ce mot est comprise dans le plateau
        /// La recherche récursive commence donc par la recherche de la deuxième lettre
        /// </summary>
        /// <param name="ligne"></param> ligne de la case courante
        /// <param name="colonne"></param> colonne de la case courante
        /// <param name="mot"></param> mot recherché
        /// <param name="indice_lettre"></param> indice du tableau contenant les lettres du mot
        /// <param name="matrice_plateau"></param> matrice repésentant le plateau de jeu
        /// <param name="matrice_cases_visitees"></param> matrice informant de l'état des cases du plateau
        /// <returns></returns>
        static bool RechercheMotRecursive (int ligne, int colonne, char [] mot, int indice_lettre, char [,] matrice_plateau, bool [,] matrice_cases_visitees)
        {
            bool rep = false;
            for (int i = ligne-1; i<= ligne+1 && (indice_lettre < mot.Length) && !rep; i++)
            {
                for (int j = colonne - 1; j <= colonne + 1 && (indice_lettre < mot.Length) && !rep; j++)
                {
                    if (i>=0 && i< matrice_plateau.GetLength(0) && j>=0 && j < matrice_plateau.GetLength(1))   // On vérifie que l'index est bien compris dans les limites de la matrice notamment pour les cases en bordure du plateau
                    {
                        if (matrice_plateau[i,j] == mot[indice_lettre] && matrice_cases_visitees[i,j] == false)
                        {
                            matrice_cases_visitees[i, j] = true;
                            if (indice_lettre+1 == mot.Length)
                            {
                                rep = true;
                            }
                            else
                            {
                                rep = RechercheMotRecursive(i, j, mot, indice_lettre + 1, matrice_plateau, matrice_cases_visitees);
                                matrice_cases_visitees[i, j] = false;    // Ceci permet de réinitialiser la case actuelle en non visitée(non prise) lorsque le chemin a avorté, la case est ainsi libérée pour un nouvel essai
                            }
                        }
                    }
                }
            }
            return rep;
        }


        /// <summary>
        /// Création d'une matrice représentant le plateau de Jeu, car il est ainsi plus simple de parcourir les cases adjacentes
        /// (Mise sous forme de matrice, le tableau contenant les faces supérieures des dés) 
        /// </summary>
        /// <returns></returns> Retourne la matrice créée
        public char [,] MatriceRepresentantPlateau()
        {
            char[,] matrice = new char[4, 4];
            int indice_plateau = 0;
            for (int i =0; i<matrice.GetLength(0); i++)
            {
                for (int j =0; j<matrice.GetLength(1); j++)
                {
                    matrice[i, j] = facesSuperieures[indice_plateau];
                    indice_plateau++;
                }
            }
            return matrice;
        }


        /// <summary>
        /// Création d'une matrice renseignant sur l'état d'une case du plateau (libre ou pas)
        /// </summary>
        /// <returns></returns> Retourne la matrice créée
        public bool [,] CasesVisitees()
        {
            bool[,] matrice = new bool[4, 4];
            for (int i =0; i<matrice.GetLength(0); i++)
            {
                for (int j =0; j < matrice.GetLength(1); j++)
                {
                    matrice[i, j] = false;      // La case n'as pas été utilisée pour la formation du mot actuel
                }
            }
            return matrice;
        }
    

    }
}
