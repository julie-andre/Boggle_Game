using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Probleme
{
    class Program
    {
     
        // Attributs
        private Dictionnaire dico;
        private Plateau plateau;

        //Constructeur
        public Program (string dico_filename, string plateau_filename, Random r)
        {
            dico = new Dictionnaire(dico_filename);
            plateau = new Plateau(plateau_filename, r);
        }

        // Propriétés
        public Dictionnaire Dico
        {
            get { return dico; }
        }

        public Plateau Plateau
        {
            get { return plateau; }
        }

        // Méthode
        /// <summary>
        /// Fonction retournant le nombre de points obtenu pour le mot correspondant
        /// </summary>
        /// <param name="mot"></param>
        /// <returns></returns>
        public int ScoreMot (string mot)
        {
            int score =0;
            if (mot.Length >= 3)
            {
                if (mot.Length <= 6)
                {
                    score = mot.Length - 1;
                }
                else
                {
                    score = 11;
                }
            }
            return score;
        }

        static void Main(string[] args)
        {
            Random r = new Random();
            // Création d'une nouvelle instance de Program(donc de jeu)
            Program Jeu = new Program("MotsPossibles.txt", "Des.txt",r);

            // Création de 2 instances de Joueurs
            Console.WriteLine("Entrez le nom du joueur1 : ");
            Joueur Joueur1 = new Joueur(Console.ReadLine());
            Console.WriteLine("Entrez le nom du joueur2 : ");
            Joueur Joueur2 = new Joueur(Console.ReadLine());

            Joueur joueur_pointeur;

            for (int i = 1; i<= 6; i++)
            {
                if (i%2 == 1)
                {
                    joueur_pointeur = Joueur1;
                }
                else
                {
                    joueur_pointeur = Joueur2;
                }
                Console.WriteLine("C'est au tour de " + joueur_pointeur.Nom + " de jouer \n");
                
                //On affiche le plateau
                Console.WriteLine(Jeu.plateau.toString());
                
                // On met en place un compteur de temps, le joueur a une minute pour trouver autant de mots qu'il peut
                DateTime datedebut = DateTime.Now;
                DateTime datefin = datedebut.AddMinutes(1);

                while (DateTime.Compare(datefin, DateTime.Now) > 0)
                {
                    // On demande de saisir un mot
                    Console.WriteLine("Saisissez un nouveau mot trouvé");

                    // L'étape où le joeur réfléchi pour trouver un mot est celle la plus susceptible d'atteindre la limite de temps accordé, on teste donc si le temps n'est pas écoulé
                    if (DateTime.Compare(datefin, DateTime.Now) <= 0)
                    {
                        Console.WriteLine("Temps écoulé");
                        Console.WriteLine(joueur_pointeur.toString());
                        break;
                    }

                    string str = Console.ReadLine();
                    // On convertit le mot entré sur la console en majuscule, si jamais il ne l'était pas
                    str = str.ToUpper();

                    // On convertit le mot en tableau de char, car certaines fonctions demandent en paramètre le mot sous cette forme
                    char[] mot = new char[str.Length];
                    for (int j = 0; j < str.Length; j++)
                    {
                        mot[j] = str[j];
                    }

                    // On vérifie que le mot est éligible (3 conditions)
                    if (mot.Length >= 3)
                    {
                        if ((Jeu.dico.RechDichoRecursif(0, Jeu.dico.SplitageLigne(mot.Length - 2).Length - 1, str)) && (Jeu.plateau.Test_PLateau(mot)))
                        {
                            // On teste si le mot est contenu dans la liste des mots déjà trouvé par le joueur au cours de la partie
                            if (!joueur_pointeur.Contain(str))
                            {
                                // On ajoute le mot trouvé à la liste de mots trouvés par le joueur
                                joueur_pointeur.Add_Mot(str);
                                // On applique la fonction comptage de points, contenue dans les méthodes de la classe Program
                                joueur_pointeur.Score += Jeu.ScoreMot(str);
                                // On affiche le score et les mots actuellement trouvés
                                Console.WriteLine(joueur_pointeur.toString());
                            }
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine(joueur_pointeur.toString() + "\n");

                // Création d'un nouveau plateau de Jeu pour le joeur suivant
                Jeu = new Program("MotsPossibles.txt", "Des.txt",r);
            }

            Console.WriteLine("\n\nFin de partie\n");
            Console.WriteLine("Le gagnant est :\n");
            if (Joueur1.Score > Joueur2.Score)
            {
                Console.WriteLine(Joueur1.Nom+"\n");
            }
            else if (Joueur1.Score < Joueur2.Score)
            {
                Console.WriteLine(Joueur2.Nom+"\n");
            }
            else
            {
                Console.WriteLine(Joueur1.Nom + " et "+ Joueur2.Nom+"\n");
            }
            Console.WriteLine(Joueur1.Nom + " a obtenu un total de " + Joueur1.Score + " points");
            Console.WriteLine(Joueur2.Nom + " a obtenu un total de " + Joueur2.Score + " points");
     
            Console.ReadKey();
        }
    }
}
