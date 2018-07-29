using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    /// <summary>
    /// This is the enum which stores different results that an account registration can give.
    /// </summary>
    public enum RegisterResult
    {
        TOO_SHORT,
        REPETITIVE,
        ALPHA_NUMERIC,
        INVALID_EMAIL,
        EMAIL_IN_USE,
        SUCCESS
    }

    /// <summary>
    /// This is the parent class for each database model that requires manual saving.
    /// </summary>
    public class MongoObject
    {
        /// <summary>
        /// This function allows you to replace a specific database model instance (filtered by unique IDs).
        /// </summary>
        /// <param name="db">The IMongoDatabase instance to use for the saving.</param>
        public virtual void ReplaceAsync(IMongoDatabase db)
        {
            if (this is DbAccount acc)
                db.GetCollection<DbAccount>("accounts").ReplaceOneAsync(o => o.Id == acc.Id, acc);
            else if (this is DbLogin login)
                db.GetCollection<DbLogin>("logins").ReplaceOneAsync(o => o.GUID == login.GUID, login);
        }
    }

    /// <summary>
    /// This is the Account database model, it stores information about the user account (currency, name and what-not).
    /// </summary>
    public class DbAccount : MongoObject
    {
        /// <summary>
        /// This is the unique Id of the account (hence the BsonId tag), this field is never changed or repeated.
        /// </summary>
        [BsonId]
        public int Id;

        /// <summary>
        /// This is the account lock, it determines whether the user has access to the game and certain requests (to deal with some possible exploits).
        /// </summary>
        [BsonElement("lock")]
        public DateTime Lock;

        /// <summary>
        /// This is the next character Id for the given account instance.
        /// </summary>
        [BsonElement("nextCharId")]
        public int NextCharId;

        /// <summary>
        /// This is the next pet Id for the given account instance.
        /// </summary>
        [BsonElement("nextPetId")]
        public int NextPetId;

        /// <summary>
        /// This is the max amount of character slots for the given account instance.
        /// </summary>
        [BsonElement("maxNumChars")]
        public int MaxNumChars;

        /// <summary>
        /// This is the amount of credits (gold) for the given account instance.
        /// </summary>
        [BsonElement("credits")]
        public int Credits;

        /// <summary>
        /// This is the amount of fortune tokens for the given account instance.
        /// </summary>
        [BsonElement("fortuneTokens")]
        public int FortuneTokens;

        /// <summary>
        /// This is the amount of credits (gold) that needs to be used for the next character slot for the given account instance.
        /// </summary>
        [BsonElement("nextCharSlotPrice")]
        public int NextCharSlotPrice;

        /// <summary>
        /// This defines whether the owner of this account has verified their email.
        /// </summary>
        [BsonElement("verifiedEmail")]
        public bool VerifiedEmail;

        /// <summary>
        /// This defines the pet yard type of the given account instance.
        /// </summary>
        [BsonElement("petYardType")]
        public int PetYardType;

        /// <summary>
        /// This defines whether the user has chosen a name.
        /// </summary>
        [BsonElement("nameChosen")]
        public bool NameChosen;

        /// <summary>
        /// This defines whether the account is eligible for the beginner package purchase.
        /// </summary>
        [BsonElement("beginnerPackageStatus")]
        public bool BeginnerPackageStatus;

        /// <summary>
        /// This defines whether the account is age verified.
        /// </summary>
        [BsonElement("isAgeVerified")]
        public bool IsAgeVerified;

        /// <summary>
        /// This defines whether the account needs to ask the its owner for security answers.
        /// </summary>
        [BsonElement("hasSecurityQuestions")]
        public bool HasSecurityQuestions;

        /// <summary>
        /// This defines whether the account needs to ask the its owner for security answers.
        /// </summary>
        [BsonElement("showSecurityQuestions")]
        public bool ShowSecurityQuestions;

        /// <summary>
        /// This array stores the security answers for the account.
        /// </summary>
        [BsonElement("securityAnswers")]
        public string[] SecurityAnswers;

        /// <summary>
        /// This list stores the owned skins for the account.
        /// </summary>
        [BsonElement("ownedSkins")]
        public List<int> OwnedSkins;

        /// <summary>
        /// This stores the best amount of fame that the account has achieved throughout its lifetime.
        /// </summary>
        [BsonElement("bestCharFame")]
        public int BestCharFame;

        /// <summary>
        /// This stores the total amount of fame earned by the account.
        /// </summary>
        [BsonElement("totalFame")]
        public int TotalFame;

        /// <summary>
        /// This stores the amount of fame currently available on the account.
        /// </summary>
        [BsonElement("fame")]
        public int Fame;

        /// <summary>
        /// This list stores all the different class stats of the account (determines unlocked classes and what-not).
        /// </summary>
        [BsonElement("classStats")]
        public List<DbClassStat> ClassStats;

        /// <summary>
        /// This list stores all the different vault chests that the account owns.
        /// </summary>
        [BsonElement("vaults")]
        public List<int[]> Vaults;

        /// <summary>
        /// This tells you whether the account is an admin type (access to the map editor and more).
        /// </summary>
        [BsonElement("admin")]
        public bool Admin;

        /// <summary>
        /// This is the name of the account.
        /// </summary>
        [BsonElement("name")]
        public string Name;
    }

    /// <summary>
    /// This is the ClassStat database model, it stores information about a specific class type (object type, best level and best fame).
    /// </summary>
    public class DbClassStat
    {
        /// <summary>
        /// This is the object type of the class object.
        /// </summary>
        [BsonElement("objectType")]
        public int ObjectType;

        /// <summary>
        /// This is the best level obtained with the class.
        /// </summary>
        [BsonElement("bestLevel")]
        public int BestLevel;

        /// <summary>
        /// This is the highest amount of fame obtained with the class.
        /// </summary>
        [BsonElement("bestFame")]
        public int BestFame;
    }

    /// <summary>
    /// This is the DbLogin database model, it stores data about an account login.
    /// </summary>
    public class DbLogin : MongoObject
    {
        /// <summary>
        /// This is the GUID/email of the stored login.
        /// </summary>
        [BsonId]
        public string GUID;

        /// <summary>
        /// This is the accountID that is attached to the stored login.
        /// </summary>
        [BsonElement("accountId")]
        public int AccountId;

        /// <summary>
        /// This is the salt string that is attatched to the account (security measure).
        /// </summary>
        [BsonElement("salt")]
        public string Salt;

        /// <summary>
        /// This is the hashed password combined together with the random salt string (security measure).
        /// </summary>
        [BsonElement("hash")]
        public string Hash;
    }
}
