using common.resources;
using log4net;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace common
{
    /// <summary>
    /// This is the Database class, it uses MongoDB as its client and provides you with many useful functions which make up the whole storage engine of the source.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Private static readonly variable, which defines the logger instance for this class.
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(Database));

        /// <summary>
        /// Private static readonly int variable, it defines the initial seed for the thread local random instance.
        /// </summary>
        static int m_seed = Environment.TickCount;

        /// <summary>
        /// Private static readonly thread local Random variable, it allows for easy thread-safety (multiple random instances) accross different threads.
        /// </summary>
        static readonly ThreadLocal<Random> m_rand = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref m_seed)));

        /// <summary>
        /// These are the guest names. One of them is randomly selected upon account creation.
        /// </summary>
        static readonly string[] m_names =
        {
            "Darq","Deyst","Itani","Ehoni","Tiar",
            "Eango","Eashy","Eati","Eendi","Lorz",
            "Gharr","Iatho","Iawa","Idrae","Rilr",
            "Laen","Oalei","Oshyu","Odaru","Yimi",
            "Lauk","Radph","Oeti","Orothi","Rayr",
            "Queq","Saylt","Scheev","Serl","Vorv",
            "Tal","Iri","Sek","Ril","Seus","Uoro",
            "Vorck","Issz","Urake","Risrr","Drac",
            "Drol","Utanu","Yangu","Zhiar"
        };

        /// <summary>
        /// This is the private MongoClient variable, it is the client instance attatched to the Database class.
        /// </summary>
        MongoClient m_client;

        /// <summary>
        /// This is the private IMongoDatabase variable, it is the database instance attatched to the MongoClient.
        /// </summary>
        IMongoDatabase m_db;

        /// <param name="settings">The settings reference which is used to connect to MongoDB (the address, port and index).</param>
        public Database(Settings settings)
        {
            log.Info("Loading Database...");
            string conn = string.Format("mongodb://{0}:{1}", 
                settings.Common.Bind, settings.Common.Port);
            Utils.Invoke(true, () => {
                m_client = new MongoClient(conn);
                m_db = m_client.GetDatabase(settings.Common.Id);
            });
            log.Info("Database loaded...");
        }

        /// <summary>
        /// This method allows you register an account, and returns any errors that might be found.
        /// </summary>
        /// <param name="guid">GUID of the new account.</param>
        /// <param name="password">Password of the new account.</param>
        public RegisterResult Register(XmlData data, string guid, string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 10)
                return RegisterResult.TOO_SHORT;

            if (password.Distinct().Count() <= 3)
                return RegisterResult.REPETITIVE;

            if (!Regex.Match(password, "^(?=.*[a-zA-Z])(?=.*[0-9])").Success)
                return RegisterResult.ALPHA_NUMERIC;

            if (string.IsNullOrWhiteSpace(guid) || !guid.Contains("@")) //No need for a more intense check.
                return RegisterResult.INVALID_EMAIL;

            if (GetLogin(guid) != null)
                return RegisterResult.EMAIL_IN_USE;

            var salt = Path.GetRandomFileName();
            var hash = GetHash(salt + password);

            var id = 0;
            do
            {
                id = m_rand.Value.Next(int.MaxValue);
            } while (GetAccount(id) != null);

            var logins = m_db.GetCollection<DbLogin>("logins");
            var accounts = m_db.GetCollection<DbAccount>("accounts");

            var login = new DbLogin
            {
                AccountId = id,
                GUID = guid.ToUpper(),
                Salt = salt,
                Hash = hash
            };

            var acc = new DbAccount
            {
                Id = login.AccountId,
                Name = m_names[m_rand.Value.Next(m_names.Length)],
                NameChosen = false,
                BeginnerPackageStatus = true,
                Credits = 0,
                FortuneTokens = 0,
                Fame = 0,
                BestCharFame = 0,
                TotalFame = 0,
                IsAgeVerified = false,
                MaxNumChars = 1,
                NextCharId = 0,
                NextPetId = 0,
                OwnedSkins = new List<int>(),
                Lock = DateTime.UtcNow,
                Admin = false,
                SecurityAnswers = new string[3],
                VerifiedEmail = false,
                HasSecurityQuestions = true,
                ShowSecurityQuestions = true,
                PetYardType = 1,
                NextCharSlotPrice = 600
            };

            foreach (var i in data.TypeToPlayer.Values)
                acc.ClassStats.Add(new DbClassStat
                {
                    BestLevel = 0,
                    BestFame = 0,
                    ObjectType = i.ObjectType
                });

            AddVault(acc, false);

            accounts.InsertOne(acc);
            logins.InsertOne(login);
            return RegisterResult.SUCCESS;
        }

        /// <summary>
        /// Returns a sample guest account (DbAccount type).
        /// </summary>
        public DbAccount GetGuest()
        {
            var acc = new DbAccount
            {
                Id = -1,
                Name = m_names[m_rand.Value.Next(m_names.Length)],
                NameChosen = false,
                BeginnerPackageStatus = true,
                Credits = 0,
                FortuneTokens = 0,
                Fame = 0,
                BestCharFame = 0,
                TotalFame = 0,
                IsAgeVerified = false,
                MaxNumChars = 1,
                NextCharId = 0,
                NextPetId = 0,
                OwnedSkins = new List<int>(),
                Lock = DateTime.UtcNow,
                Admin = false,
                SecurityAnswers = new string[3],
                VerifiedEmail = false,
                HasSecurityQuestions = true,
                ShowSecurityQuestions = true,
                PetYardType = 1,
                NextCharSlotPrice = 600
            };
            return acc;
        }

        /// <summary>
        /// Adds one vault to the specified account.
        /// </summary>
        /// <param name="acc">The account you want to add a new vault to.</param>
        /// <param name="replace">Whether you want the value to be updated in the database immediately.</param>
        public void AddVault(DbAccount acc, bool replace = true)
        {
            if (acc.Vaults == null)
                acc.Vaults = new List<int[]>();
            acc.Vaults.Add(new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 });
            if (replace)
                acc.ReplaceAsync(m_db);
        }

        /// <summary>
        /// Converts a string to a SHA1Managed hash.
        /// </summary>
        /// <param name="text">The string you want to one way hash.</param>
        private string GetHash(string text) =>
            Convert.ToBase64String(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(text)));

        /// <summary>
        /// Acquires an account from the database based on its Id, if the account is not found, null is returned.
        /// </summary>
        /// <param name="id">Id of the account you want to find.</param>
        public DbAccount GetAccount(int id) =>
            m_db.GetCollection<DbAccount>("accounts").Find(o => o.Id == id).FirstOrDefault();

        /// <summary>
        /// Acquires a login from the database based on its GUID, it the login is not found, null is returned.
        /// </summary>
        /// <param name="guid">The GUID you want to find the login of.</param>
        public DbLogin GetLogin(string guid)
        {
            guid = guid.ToUpper();
            return m_db.GetCollection<DbLogin>("logins").Find(o => o.GUID == guid).FirstOrDefault();
        }
    }
}
