using System;
using System.Collections.Generic;
using System.Linq;
using BCrypt;


/*
 *  TODO
 *  [x] Update User
 *  [x] Search User
 *  [x] Forgot Password
 */

namespace TugasKelompok
{
    class Program
    {
        public static List<User> users = new List<User>();
        public static int input = 0;

        public static string hashedPassword = "";

        static void Main(string[] args)
        {

            users.Add(new User { firstName = "Hasbi", lastName = "Shuhada", password = "hs123", username = "hash" });
            users.Add(new User { firstName = "Hasbi", lastName = "Orton", password = "ro123", username = "reor" });
            users.Add(new User { firstName = "John", lastName = "Cena", password = "jc123", username = "joce" });
            users.Add(new User { firstName = "Rina", lastName = "Nose", password = "rn123", username = "rino" });
            Menu();
        }

        static void Menu()
        {
            Console.Clear();
            Console.WriteLine("==BASIC AUTHENTICATION==");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Show User");
            Console.WriteLine("3. Delete User");
            Console.WriteLine("4. Update User");
            Console.WriteLine("5. Search");
            Console.WriteLine("6. Login");
            Console.WriteLine("7. Forgot Password");
            Console.WriteLine("8. Exit");

            try
            {
                Console.Write("Input Menu : ");
                input = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (input)
                {
                    case 1:
                        Console.Clear();
                        Create();
                        break;
                    case 2:
                        Console.Clear();
                        Show();
                        break;
                    case 3:
                        DeleteUser();
                        break;
                    case 4:
                        UpdateUser();
                        break;
                    case 5:
                        SearchUser();
                        break;
                    case 6:
                        Login();
                        break;
                    case 7:
                        ForgotPassword();
                        break;
                    case 8:
                        Console.WriteLine("Terima Kasih Telah Menggunakan Aplikasi Kami");
                        System.Environment.Exit(1);
                        break;
                    default:
                        Console.WriteLine("Error: Input not Valid");
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
            BackToMenu();
        }

        static void Create()
        {
            Console.WriteLine("==Create User==");
            Console.Write("First Name : ");
            string firstName = Console.ReadLine();
            Console.Write("Last Name : ");
            string lastName = Console.ReadLine();
            Console.Write("Password : ");
            string password = Console.ReadLine();
            
            hashedPassword = PasswordEnc(password);

            string userName = string.Concat(firstName.Substring(0, 2), lastName.Substring(0, 2)).ToLower();

            // compare username in list
            string usernameCompare = Compare(userName);

            users.Add(new User { firstName = firstName, lastName = lastName, password = hashedPassword, username = usernameCompare });
            BackToMenu();
        }

        static void Show()
        {
            Console.Clear();
            Console.WriteLine("==SHOW USER==");

            if (users.Count != 0)
            {
                for (int k = 0; k < users.Count; k++)
                {
                    Console.WriteLine("======================");
                    Console.WriteLine($"NAME : {users[k].firstName} {users[k].lastName}");
                    Console.WriteLine($"USERNAME : {users[k].username}");
                    Console.WriteLine($"PASSWORD : {users[k].password}");
                    Console.WriteLine("======================");
                }
                BackToMenu();
            }
            else
            {
                Console.WriteLine("Belum ada user yang terdaftar, silahkan register");
                BackToMenu();
            }
        }

        public static void BackToMenu()
        {
            Console.ReadKey(true);
            Menu();
        }

        public static string PasswordEnc(string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;
        }

        public static string Compare(string username)
        {
            //Compare username;
            bool compareUsername = users.Exists(user => user.username == username);

            //Generate random number
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            
            // if username exist/true
            if (compareUsername)
            {
                // concat between username and random number
                return string.Concat(username, randomNumber);
            }

            return username;
        }

        public static void Login()
        {
            Console.Clear();
            
            Console.WriteLine("==LOGIN==");
            Console.Write("USERNAME : ");
            string inputUsername = Console.ReadLine();
            Console.Write("PASSWORD : ");
            string inputPassword = Console.ReadLine();

            bool comparelogin = users.Exists(user => user.username == inputUsername);

            bool validPassword = BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);

            // if username and password match 
            if (comparelogin)
            {
                if (validPassword)
                {
                    Console.WriteLine("Login Successfuly");
                }
                else
                {
                    Console.WriteLine("Password not Match");
                }
            }
            else
            {
                Console.WriteLine("Login Failed");
            }
            BackToMenu();
        }

        public static void DeleteUser()
        {
            if (users.Count != 0)
            {
                for (int k = 0; k < users.Count; k++)
                {
                    Console.WriteLine("======================");
                    Console.WriteLine($"NAME : {users[k].firstName} {users[k].lastName}");
                    Console.WriteLine($"USERNAME : {Compare(users[k].username)}");
                    Console.WriteLine($"PASSWORD : {users[k].password}");
                    Console.WriteLine("======================");
                }

                Console.WriteLine("Masukan index User yang akan dihapus");
                int index = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Apakah anda yakin?");
                Console.WriteLine("Pilih ya|tidak");

                string deleteOption = Console.ReadLine();

                if (deleteOption.Equals("YA") || deleteOption.Equals("ya"))
                {
                    users.RemoveAt(index - 1);

                    Console.WriteLine("User Terhapus");
                }
                else
                {
                    BackToMenu();
                }
                BackToMenu();
            }
            else
            {
                Console.WriteLine("Belum ada user yang terdaftar, silahkan register");
                BackToMenu();
            }
        }

        // TODO
        public static void UpdateUser()
        {
            Console.Write("Masukan data yang ingin diubah (firstname): ");
            string target = Console.ReadLine();

            Console.Write("Masukan pasword yang baru: ");
            string passwordInput = Console.ReadLine();

            Console.Write("Masukan firstname yang baru: ");
            string firstNameInput = Console.ReadLine();

            Console.Write("Masukan lastname yang baru: ");
            string lastNameInput = Console.ReadLine();


            hashedPassword = PasswordEnc(passwordInput);

            string userName = string.Concat(firstNameInput.Substring(0, 2), lastNameInput.Substring(0, 2)).ToLower();

            string usernameCompare = Compare(userName);

            User result = users.FirstOrDefault(item => item.firstName == target);

            if (result != null)
            {
                result.firstName = firstNameInput;
                result.lastName = lastNameInput;
                result.password = hashedPassword;
                result.username = usernameCompare;
            }
        }

        public static void ForgotPassword()
        {
            Console.Write("Masukan data yang ingin diubah (firstname): ");
            string target = Console.ReadLine();

            Console.Write("Masukan pasword yang baru: ");
            string passwordInput = Console.ReadLine();

            User result = users.FirstOrDefault(item => item.firstName == target);

            if (result != null)
            {
                result.password = passwordInput;
            }
        }

        public static void SearchUser()
        {
            Console.Write("Masukan data yang dicari: ");
            string target = Console.ReadLine();

            List<User> result = users.FindAll(user => user.firstName.Equals(target));
            
            Console.WriteLine($"Ditemukan {result.Count} Data");
            for (int i = 0; i < result.Count; i++)
            {
                Console.WriteLine("==============================");
                Console.WriteLine($"First Name: {result[i].firstName}");
                Console.WriteLine($"Last Name: {result[i].lastName}");
                Console.WriteLine($"Username: {result[i].username}");
                Console.WriteLine($"Password: {result[i].password}");
                Console.WriteLine("==============================");
            }
        }
    }
}
