using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace GoodServer.Data
{
    

    public class UserManager
    {
        public async Task<bool> UpdateAsync(User user)
        {
            bool result = false;
            await Task.Run(() =>
            {
                lock (sync_lock)
                {
                    LoadDictionary();
                    foreach (var u in _users)    //Loop through the list until you find the user with the matching email
                    {
                        if (u.Email == user.Email)  //Update the first and last name of this user
                        {
                            u.FirstName = user.FirstName;
                            u.LastName = user.LastName;
                            u.PictureUrl = user.PictureUrl;
                            result = true;
                            break;

                        }
                    }
                    SaveDictionary();
                }

            });
            return result;
        }

        private static List<User> _users = null;

        private const string filename = "users.xml";

        private static object sync_lock = new object();

        /// <summary>
        /// Call this before attempting to access _users in this class
        /// </summary>
        private void LoadDictionary()
        {
            if (System.IO.File.Exists(filename))
            {
                {
                    System.IO.FileStream stream = System.IO.File.OpenRead(filename);
                    System.Xml.Serialization.XmlSerializer serializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(List<User>));
                    _users = (List<User>)serializer.Deserialize(stream);
                    stream.Dispose();
                }
            }
            else
            {
                _users = new List<User>();
            }
        }

        /// <summary>
        /// Call this after changing _users
        /// </summary>
        private void SaveDictionary()
        {
            System.IO.FileStream stream = System.IO.File.OpenWrite(filename);
            System.Xml.Serialization.XmlSerializer serializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(List<User>));
            serializer.Serialize(stream, _users);
            stream.Dispose();
        }


        public async Task<bool> CreateAsync(string email, string password)
        {
            bool result = false;
            await Task.Run(() =>
            {
                lock (sync_lock)
                {
                    LoadDictionary();
                    if (!_users.Any(x => x.Email == email))
                    {
                        _users.Add(new User { Email = email, Password = password });
                        SaveDictionary();
                        result = true;
                    }
                }
            });
            return result;
        }

        public async Task<bool> CheckPasswordAsync(string email, string password)
        {
            bool result = false;
            await Task.Run(() =>
            {
                lock (sync_lock)
                {
                    LoadDictionary();
                    if (_users.Any(x => x.Email == email))
                    {
                        if (_users.Single(x => x.Email == email).Password == password)
                        {
                            result = true;
                        }
                    }

                }
            });
            return result;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            User result = null;
            await Task.Run(() =>
            {
                lock (sync_lock)
                {
                    LoadDictionary();
                    if (_users.Any(x => x.Email == email))
                    {
                        result = _users.Single(x => x.Email == email);
                    }
                }
            });

            return result;
        }
    }
}
