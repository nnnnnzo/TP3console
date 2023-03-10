using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore;
using System.IO.Pipes;
using System.Linq;
using TP3console.Models.EntityFramework;

namespace TP3console
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //using (var ctx = new FilmsDBContext())
            //{
            //    Film titanic = ctx.Films.First(f => f.Nom.Contains("Titanic"));

            //    titanic.Description = "Un bateau échoué. Date : " + DateTime.Now;

            //    int nbchanges = ctx.SaveChanges();

            //    Console.WriteLine("Nombre d'enregistrements modifiés ou ajoutés " + nbchanges);
            //}
            //Console.ReadKey();

            //using (var ctx = new FilmsDBContext())
            //{
            //    //Chargement de la catégorie Action et des films de cette catégorie
            //    Categorie categorieAction = ctx.Categories.Include(c => c.Films).ThenInclude(f => f.Avis).First(c => c.Nom == "Action");
            //    Console.WriteLine("Categorie : " + categorieAction.Nom);
            //    Console.WriteLine("Films : ");
            //    foreach (var film in categorieAction.Films)
            //    {
            //        Console.WriteLine(film.Nom);
            //    }
            //}

            Exo2Q1();
            Exo2Q2();
            Exo2Q3();
            Exo2Q4();
            Exo2Q5();
            Exo2Q6();
            Exo2Q7();
            Exo2Q8();
            Exo2Q9();
            AddUser();
            Edit();
            Delete();
            AddAvis();
            AddRng();


            Console.ReadKey();
        }

        public static void Exo2Q1()
        {
            var ctx = new FilmsDBContext();
            foreach (var film in ctx.Films)
            {
                Console.WriteLine(film.ToString());
            }
        }
        public static void Exo2Q2()
        {
            var ctx = new FilmsDBContext();
            foreach (var utilisateur in ctx.Utilisateurs)
            {
                Console.WriteLine(utilisateur.Email);
            }
        }
        public static void Exo2Q3()
        {
            var ctx = new FilmsDBContext();
            foreach (var utilisateur in ctx.Utilisateurs.OrderBy(x => x.Login))
            {
                Console.WriteLine(utilisateur.Login);
            }
        }

        public static void Exo2Q4()
        {
            var ctx = new FilmsDBContext();
            Categorie categorieAction = ctx.Categories.Include(c => c.Films).First(c => c.Nom == "Action");
            foreach (var Film in categorieAction.Films)
            {
                Console.WriteLine(Film.Id + " "+ Film.Nom);
            }
        }

        public static void Exo2Q5()
        {
            var ctx = new FilmsDBContext();
            Console.WriteLine(ctx.Categories.Count());

        }

        public static void Exo2Q6()
        {
            var ctx = new FilmsDBContext();
            var noteMin = ctx.Avis.Min(x => x.Note);
            Console.WriteLine(noteMin);
        }

        public static void Exo2Q7()
        {
            var ctx = new FilmsDBContext();
            var films = ctx.Films.Where(f => f.Nom.ToLower().StartsWith("le"));
            foreach(var f in films)
            {
                Console.WriteLine(f.Nom);
            }

        }

        public static void Exo2Q8()
        {
            var ctx = new FilmsDBContext();
            //Pour que cela marche, il faut que la requête envoie les mêmes noms de colonnes que les classes c#.
            decimal moyenne = 0;
            int count = 0;
            foreach (var avi in ctx.Avis.Where(film => film.FilmNavigation.Nom.ToLower()=="pulp fiction").ToList())
            {
                count++;
                moyenne += avi.Note;
            }
            moyenne /= count;
            Console.WriteLine(moyenne);
        }

        public static void Exo2Q9()
        {
            var ctx = new FilmsDBContext();
            decimal noteMax = 0;
            string user = "";
            foreach (var avi in ctx.Avis.ToList())
            {
                if (avi.Note > noteMax)
                {
                    noteMax = avi.Note;
                    user = avi.Utilisateur.ToString();
                }
                    

            }
            Console.WriteLine(user);
        }

        public static void AddUser()
        {
            var ctx = new FilmsDBContext();
            Utilisateur moi = new Utilisateur();
            moi.Login = "lagadece";
            moi.Email = "email1@test.fr";
            moi.Pwd= "password";

            ctx.Utilisateurs.Add(moi);
            ctx.SaveChanges();
        }

        public static void Edit() {
            var ctx = new FilmsDBContext();

            var films = ctx.Films.First(film => film.Nom.ToLower() == "l'armee des douze singes");
            films.Description = "desc test";
            ctx.SaveChanges();

        }

        public static void Delete()
        {
            var ctx = new FilmsDBContext();
            var films = ctx.Films.First(film => film.Nom.ToLower() == "l'armee des douze singes");

            foreach (var avis in ctx.Avis.Where(s => s.Film == films.Id))
            {
                ctx.Avis.Remove(avis);
            }
            ctx.Films.Remove(films);
            ctx.SaveChanges();
        }

        public static void AddAvis() { 
            var ctx = new FilmsDBContext();
            var films = ctx.Films.First(film => film.Nom.ToLower() == "blade runner");
            Avi avi = new Avi();
            var userMoi = ctx.Utilisateurs.First(user => user.Login.ToLower() == "lagadece");
            avi.Utilisateur = userMoi.Id;
            avi.Film = films.Id;
            avi.Avis = "Masterclass";
            avi.Note = (decimal)0.99;
            avi.UtilisateurNavigation = userMoi;
            avi.FilmNavigation = films;

            ctx.Avis.Add(avi);
            ctx.SaveChanges();

        }

        public static void AddRng() { 
            var ctx = new FilmsDBContext();
            var catDrame = ctx.Categories.First(cat => cat.Nom.ToLower() == "drame");


            Film film1 = new Film();
            Film film2 = new Film();

            film1.Nom = "Bullet Train";
            film1.Description = "Cinq tueurs à gages se retrouvent dans un train à grande vitesse voyageant entre Tokyo et Morioka et ne faisant que très peu d'arrêts. Les cinq criminels vont découvrir qu'ils sont tous liés par leur mission, se demandent qui va en sortir vivant et s'interrogent sur ce qui les attend à la gare d'arrivée.";
            film1.Categorie = catDrame.Id;
            film1.CategorieNavigation = catDrame;

            film2.Nom = "The Batman";
            film2.Description = "Dans sa deuxième année de lutte contre le crime, le milliardaire et justicier masqué Batman explore la corruption qui sévit à Gotham et notamment comment elle pourrait être liée à sa propre famille, les Wayne, à qui il doit toute sa fortune. En parallèle, il enquête sur les meurtres d'un tueur en série qui se fait connaître sous le nom de Sphinx et sème des énigmes cruelles sur son passage.";
            film2.Categorie = catDrame.Id;
            film2.CategorieNavigation = catDrame;

            ctx.Films.AddRange(film1,film2);
            ctx.SaveChanges();
        }


    }
}