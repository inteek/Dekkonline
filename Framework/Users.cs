using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Framework.Libraies;
using System.Data.Entity;
using System.Security.Cryptography;

namespace Framework
{
    public class Users
    {
        //DE-14 1
        public string loadAddressUser(string idUser)
        {
            var UserAddress = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    UserAddress = (from user in db.UserAddress where user.IdUser == idUser select new { adreess = user.Address, latitude = user.Latitude, length = user.Length }).FirstOrDefault().ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return UserAddress;
        }

        //DE-14 2
        public bool updateAddressUser(string idUser, string firstName, string lastName, string address, string phone, int zipCode, string latitude, string length)
        {
            bool Result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var result = db.UserAddress.Where(s => s.IdUser == idUser).FirstOrDefault();
                    if (result != null)
                    {
                        result.FirstName = firstName;
                        result.LastName = lastName;
                        result.Address = address;
                        result.Phone = phone;
                        result.ZipCode = zipCode;
                        result.Latitude = latitude;
                        result.Length = length;

                        db.Entry(result).State = EntityState.Modified;
                        db.SaveChanges();
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Result;
        }

        //DE-14 5
        public bool addToUserAddress(string idUser, string firstName, string lastName, string address, string phone, int zipCode, string latitude, string length)
        {
            bool result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var user = db.UserAddress.Where(s => s.IdUser == idUser).FirstOrDefault();
                    if (user == null)
                    {
                        var addUserAddress = new Entity.UserAddress();
                        addUserAddress.IdUser = idUser;
                        addUserAddress.FirstName = firstName;
                        addUserAddress.LastName = lastName;
                        addUserAddress.Address = address;
                        addUserAddress.Phone = phone;
                        addUserAddress.ZipCode = zipCode;
                        addUserAddress.Latitude = latitude;
                        addUserAddress.Length = length;
                        db.UserAddress.Add(addUserAddress);
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        user.FirstName = firstName;
                        user.LastName = lastName;
                        user.Address = address;
                        user.Phone = phone;
                        user.ZipCode = zipCode;
                        user.Latitude = latitude;
                        user.Length = length;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        //DE-23 1
        public List<ResultDataUser> dataUser(string idUser)
        {

            List<ResultDataUser> user = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                   
                    user = (from us in db.UserAddress
                            join aspuser in db.AspNetUsers on us.IdUser equals aspuser.Id
                            where us.IdUser == idUser
                            select new ResultDataUser
                            {
                                IdUser = us.IdUser,
                                FirstName = us.FirstName,
                                LastName = us.LastName,
                                Address = us.Address,
                                Phone = us.Phone,
                                ZipCode = us.ZipCode.ToString(),
                                Latitude = us.Latitude,
                                length = us.Length,
                                Image = us.Image,
                                Email = aspuser.Email
                            }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }

        public bool validaLogin(string user, string pass)
        {
            bool valUser = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var result = db.AspNetUsers.Where(s => s.UserName == user && s.PasswordHash == pass).FirstOrDefault();

                    if (result != null)
                    {
                        valUser = true;
                    }
                    else
                    {
                        valUser = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return valUser;
        }

        public bool addNetUser(string email)
        {
            return true;
        }



        // Define default min and max password lengths.
        private  int DEFAULT_MIN_PASSWORD_LENGTH = 7;
        private  int DEFAULT_MAX_PASSWORD_LENGTH = 12;

        // Define supported password characters divided into groups.
        // You can add (or remove) characters to (from) these groups.
        private  string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        private  string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        private  string PASSWORD_CHARS_NUMERIC = "23456789";
        private  string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";


        public string GeneratePassword()
        {
            int minLength = DEFAULT_MIN_PASSWORD_LENGTH;
            int maxLength = DEFAULT_MAX_PASSWORD_LENGTH;

            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][]
            {
            PASSWORD_CHARS_LCASE.ToCharArray(),
            PASSWORD_CHARS_UCASE.ToCharArray(),
            PASSWORD_CHARS_NUMERIC.ToCharArray(),
            PASSWORD_CHARS_SPECIAL.ToCharArray()
            };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = BitConverter.ToInt32(randomBytes, 0);

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;

            // Allocate appropriate memory for the password.
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            // Index of the next character to be added to password.
            int nextCharIdx;

            // Index of the next character group to be processed.
            int nextGroupIdx;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx;

            // Index of the last non-processed character in a group.
            int lastCharIdx;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.
            for (int i = 0; i < password.Length; i++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                // Add this character to the password.
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                // There are more unprocessed characters left.
                else
                {
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    // Decrement the number of unprocessed characters in
                    // this group.
                    charsLeftInGroup[nextGroupIdx]--;
                }

                // If we processed the last group, start all over.
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                // There are more unprocessed groups left.
                else
                {
                    // Swap processed group with the last unprocessed group
                    // so that we don't pick it until we process all groups.
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            return new string(password);
        }



        public string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ0123456789?!@.-_";
            Byte[] randomBytes = new Byte[PasswordLength];
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                Random randomObj = new Random();
                randomObj.NextBytes(randomBytes);
                if (i < (PasswordLength - 1))
                {
                    chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
                }
                else {
                    chars[i] = '_';
                }
            }

            return new string(chars);
        }
        public string IdUser(string email)
        {
            var user = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    user = db.AspNetUsers.Where(s => s.UserName == email).Select(s => s.Id).FirstOrDefault().ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return user;
        }

        public List<ResulUserWorkShop> infoStep2(string User)
        {
            List<ResulUserWorkShop> result = null;
            List<ResultWorkshop> listWorkshop = null;
            var userAddress = (dynamic)null;

            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    userAddress = db.UserAddress.Where(s => s.IdUser == User).FirstOrDefault();
                    string userEmail = db.AspNetUsers.Where(s => s.Id == User).Select(s => s.UserName).FirstOrDefault();

                    Workshop workshop = new Workshop();

                    listWorkshop = workshop.loadWorkshopAddress(userAddress.ZipCode);

                    if (userAddress != null || userAddress != "")
                    {
                        result = new List<ResulUserWorkShop> {
                        new ResulUserWorkShop{
                        workshop = listWorkshop,
                        zipcode = userAddress.ZipCode,
                        firstName = userAddress.FirstName,
                        lastName = userAddress.LastName,
                        address = userAddress.Address,
                        email = userEmail,
                        mobile = userAddress.Phone} };
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public bool ValidateUserEmail(string email1, string iduser)
        {
            bool result = false;
            try
            {
                using (dekkOnlineEntities db = new dekkOnlineEntities())
                {
                    var emailuser = db.AspNetUsers.Where(s => s.Id == iduser).Select(s=>s.Email).FirstOrDefault().ToString();
                    var emailnew = db.AspNetUsers.Where(s=>s.Email == email1).Select(s => s.Email).FirstOrDefault().ToString();
                    if (emailnew != null && emailuser != emailnew)
                    {
                        result = true;
                    }
                    else if (emailnew == null || emailuser == emailnew)
                    {
                        result = false;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public bool UpdateDataUser(string zipcore, string name, string lastname, string address, string email, string mobile, string idUser1) {
            bool result = false;
            try
            {
                using (dekkOnlineEntities db = new dekkOnlineEntities())
                {
                    var userdata = db.UserAddress.Where(s => s.IdUser == idUser1).FirstOrDefault();
                    var useremail = db.AspNetUsers.Where(s => s.Id == idUser1).FirstOrDefault();
                    if (userdata != null && useremail != null)
                    {
                        userdata.FirstName = name;
                        userdata.LastName = lastname;
                        userdata.Address = address;
                        userdata.Phone = mobile;
                        userdata.ZipCode = Convert.ToInt32(zipcore);
                        useremail.Email = email;
                        useremail.UserName = email;
                        db.Entry(userdata).State = EntityState.Modified;
                        db.Entry(useremail).State = EntityState.Modified;
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public bool UpdateUserImage(string path, string idUser1)
        {
            bool result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var UserInfo = db.UserAddress.Where(s => s.IdUser == idUser1).FirstOrDefault();
                    if (UserInfo != null)
                    {
                        UserInfo.Image = path;
                        db.Entry(UserInfo).State = EntityState.Modified;
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }

    }
}
