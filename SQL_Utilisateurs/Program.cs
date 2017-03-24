using MySql.Data.MySqlClient;
using System;

namespace SQL_Utilisateurs
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Créer d'un utilisateur à ajouter
            Utilisateurs utilisateur = new Utilisateurs();
            utilisateur.Id = 1;
            utilisateur.Nom = "Dupond";
            utilisateur.Prenom = "Harry";
            utilisateur.Tel = "0365985246";
            utilisateur.Ville = "Douai";

            // Création de l'objet Bdd pour l'intéraction avec la base de donnée MySQL
            Bdd bdd = new Bdd();
            bdd.OuvreConnexion();
            Console.WriteLine("Connexion ouverte");
            Console.WriteLine("Ajout de 4 utilisateurs :");

            bdd.AddUtilisateur(utilisateur);

            utilisateur.Id = 2;
            utilisateur.Nom = "Martin";
            utilisateur.Prenom = "Sally";
            utilisateur.Tel = "0654859617";
            utilisateur.Ville = "Lille";
            bdd.AddUtilisateur(utilisateur);

            utilisateur.Id = 3;
            utilisateur.Nom = "Milner";
            utilisateur.Prenom = "Edith";
            utilisateur.Tel = "0456321685";
            utilisateur.Ville = "Roubaix";
            bdd.AddUtilisateur(utilisateur);

            utilisateur.Id = 4;
            utilisateur.Nom = "Lawry";
            utilisateur.Prenom = "James";
            utilisateur.Tel = "0365857631";
            utilisateur.Ville = "Douai";
            bdd.AddUtilisateur(utilisateur);
            bdd.LireUtilisateur();

            bdd.FermeConnexion();
            Console.ReadLine();

            Console.WriteLine("Supprime la ligne 3");
            bdd.OuvreConnexion();

            utilisateur.Id = 3;
            utilisateur.Nom = "Milner";
            utilisateur.Prenom = "Edith";
            utilisateur.Tel = "0456321685";
            utilisateur.Ville = "Roubaix";
            bdd.SupprUtilisateur(utilisateur);
            bdd.FermeConnexion();

            bdd.OuvreConnexion();
            Console.WriteLine("Lecture des 3 utilisateurs restants :");
            bdd.LireUtilisateur();
            bdd.FermeConnexion();

            Console.ReadLine();

            Console.WriteLine("Modifie le nom de Sally par Dupont (id=2)");
            bdd.OuvreConnexion();
            utilisateur.Id = 2;
            utilisateur.Nom = "Dupont";
            utilisateur.Prenom = "Sally";
            utilisateur.Tel = "0654859617";
            utilisateur.Ville = "Lille";
            bdd.UpdateUtilisateur(utilisateur);
            bdd.FermeConnexion();

            Console.WriteLine("Vérification de la modification :");
            bdd.OuvreConnexion();
            bdd.LireUtilisateur();
            bdd.FermeConnexion();

            Console.ReadLine();

            bdd.OuvreConnexion();
            bdd.AddUtilisateur(utilisateur);
            utilisateur.Id = 3;
            utilisateur.Nom = "Milner";
            utilisateur.Prenom = "Edith";
            utilisateur.Tel = "0456321685";
            utilisateur.Ville = "Toulon";
            bdd.AddUtilisateur(utilisateur);

            string reponse;
            Console.WriteLine("\n" + "Voulez-vous vider la table (O/N) ?");
            reponse = Console.ReadLine();

            if (reponse.ToUpper() == "O")
            {
                bdd.OuvreConnexion();
                bdd.SupprAll(utilisateur);
                Console.WriteLine("Table vidée !");
            }

            Console.WriteLine("Connexion fermée");
            bdd.FermeConnexion();

            Console.WriteLine("\n" + "Appuyer sur une touche pour fermer l'application...");
            Console.ReadLine();
        }
    }

    public class Utilisateurs
    {
        //  Création des propriétés
        public int Id { get; set; }

        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Tel { get; set; }
        public string Ville { get; set; }

        //  Constructeur
        public Utilisateurs()
        {
        }
    }

    public class Bdd
    {
        private MySqlConnection connection;
        private MySqlDataAdapter MyAdapter;
        private MySqlDataReader rdr;

        // Constructeur
        public Bdd()
        {
            this.InitConnexion();
        }

        // Méthode pour initialiser la connexion
        private void InitConnexion()
        {
            // Création de la chaîne de connexion
            string connectionString = "SERVER=127.0.0.1; DATABASE=utilisateur; UID=root; PASSWORD=";
            this.connection = new MySqlConnection(connectionString);
            this.MyAdapter = new MySqlDataAdapter();
            this.rdr = null;
        }

        public void OuvreConnexion()
        {
            this.connection.Open();
        }

        public void FermeConnexion()
        {
            this.connection.Close();
        }

        // Méthode pour ajouter un utilisateur
        public void AddUtilisateur(Utilisateurs utilisateurs)
        {
            try
            {
                // Création d'une commande SQL en fonction de l'objet connection
                MySqlCommand cmd = this.connection.CreateCommand();

                // Requête SQL
                cmd.CommandText = "INSERT INTO Utilisateurs (id, nom, prenom, tel, ville) VALUES (@id, @nom, @prenom, @tel, @ville)";

                // utilisation de l'objet contact passé en paramètre
                cmd.Parameters.AddWithValue("@id", utilisateurs.Id);
                cmd.Parameters.AddWithValue("@nom", utilisateurs.Nom);
                cmd.Parameters.AddWithValue("@prenom", utilisateurs.Prenom);
                cmd.Parameters.AddWithValue("@tel", utilisateurs.Tel);
                cmd.Parameters.AddWithValue("@ville", utilisateurs.Ville);

                // Exécution de la commande SQL
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Méthode pour lire un utilisateur
        public void LireUtilisateur()
        {
            // Création d'une commande SQL en fonction de l'objet connection
            MySqlCommand cmd = this.connection.CreateCommand();

            // Requête SQL
            cmd.CommandText = "SELECT * FROM Utilisateurs;";

            MyAdapter.SelectCommand = new MySqlCommand(cmd.CommandText, this.connection);

            rdr = MyAdapter.SelectCommand.ExecuteReader();

            if (rdr != null)
            {
                while (rdr.Read())
                {
                    int iid = Int32.Parse(rdr["id"].ToString());
                    string snom = rdr["nom"].ToString();
                    string sprenom = rdr["prenom"].ToString();
                    string stel = rdr["tel"].ToString();
                    string sville = rdr["ville"].ToString();
                    Console.WriteLine("id = " + iid + " nom = " + snom + " prenom= " + sprenom + " tel= " + stel + " ville= " + sville);
                }
            }
        }

        // Méthode pour supprimer un utilisateur
        public void SupprUtilisateur(Utilisateurs utilisateurs)
        {
            try
            {
                // Création d'une commande SQL en fonction de l'objet connection
                MySqlCommand cmd = this.connection.CreateCommand();

                // Requête SQL
                cmd.CommandText = "DELETE FROM Utilisateurs WHERE id= @id;";

                // utilisation de l'objet contact passé en paramètre
                cmd.Parameters.AddWithValue("@id", utilisateurs.Id);
                cmd.Parameters.AddWithValue("@nom", utilisateurs.Nom);
                cmd.Parameters.AddWithValue("@prenom", utilisateurs.Prenom);
                cmd.Parameters.AddWithValue("@tel", utilisateurs.Tel);
                cmd.Parameters.AddWithValue("@ville", utilisateurs.Ville);

                // Exécution de la commande SQL
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Méthode pour update un utilisateur
        public void UpdateUtilisateur(Utilisateurs utilisateurs)
        {
            try
            {
                // Création d'une commande SQL en fonction de l'objet connection
                MySqlCommand cmd = this.connection.CreateCommand();

                // Requête SQL
                cmd.CommandText = "UPDATE Utilisateurs SET nom= @nom WHERE id= @id;";

                // utilisation de l'objet contact passé en paramètre
                cmd.Parameters.AddWithValue("@id", utilisateurs.Id);
                cmd.Parameters.AddWithValue("@nom", utilisateurs.Nom);
                cmd.Parameters.AddWithValue("@prenom", utilisateurs.Prenom);
                cmd.Parameters.AddWithValue("@tel", utilisateurs.Tel);
                cmd.Parameters.AddWithValue("@ville", utilisateurs.Ville);

                // Exécution de la commande SQL
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Méthode pour tout supprimer d'une table
        public void SupprAll(Utilisateurs utilisateurs)
        {
            try
            {
                // Création d'une commande SQL en fonction de l'objet connection
                MySqlCommand cmd = this.connection.CreateCommand();

                // Requête SQL
                cmd.CommandText = "DELETE FROM Utilisateurs;";

                // Exécution de la commande SQL
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}