using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;


namespace ReferralApi.Models
{
    public class User
    {
        private const int _minPasswordLength = 8;
        private const int _refCodeLength = 8;
        private const string _refCodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public int id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatorio")]
        [StringLength(100, ErrorMessage = "Nome não pode exceder 100 caracteres")]
        public string name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string email { get; set; } = string.Empty;

        public string passwordHash { get; private set; } = string.Empty;
        public string refCode { get; private set; } = string.Empty;
        public int points { get; private set; }

        public int? referredById { get; set; }
        public virtual User? referredBy { get; set; }
        public DateTime createdAt { get; private set; }
        public DateTime? updatedAt { get; private set; }

        protected User()
        {
            createdAt = DateTime.UtcNow;
        }

        public User(string name, string email, string password) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome não pode ser vazio", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio", nameof(email));

            this.name = name.Trim();
            this.email = email.Trim().ToLower(); 
            SetPassword(password);
            GenerateRefCode();
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("Senha não pode ser vazia", nameof(password));
            if (password.Length < _minPasswordLength) throw new ArgumentNullException($"Senha deve ter pelo menos {_minPasswordLength} caracteres", nameof(password));

            passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            UpdateTimeStamp();
        }

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        private void GenerateRefCode()
        {
            var bytes = new byte[_refCodeLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            refCode = new string(bytes.Select(b => _refCodeChars[b % _refCodeChars.Length]).ToArray());
        }

        public void RegenerateRefCode()
        {
            GenerateRefCode(); 
        }
        public void AddPoints(int pointsToAdd)
        {
            if (pointsToAdd <= 0) throw new ArgumentOutOfRangeException("Pontos devem ser positivos", nameof(pointsToAdd));

            points += pointsToAdd;
            UpdateTimeStamp();
        }
        public void RemovePoints(int pointsToRemove)
        {
            if (pointsToRemove <= 0) throw new ArgumentOutOfRangeException("Pontos devem ser positivos", nameof(pointsToRemove));
            if (points < pointsToRemove) throw new ArgumentOutOfRangeException("Pontos insuficientes");

            points -= pointsToRemove;
            UpdateTimeStamp();
        }

        public void ResetPoints()
        {
            points = 0;
            UpdateTimeStamp();
        }
        public void UpdateInfo(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Nome não pode ser vazio", nameof(name));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("Email não pode ser vazio", nameof(email));

            this.name = name;
            this.email = email;
            UpdateTimeStamp();
        }
        private void UpdateTimeStamp()
        {
            updatedAt = DateTime.UtcNow;
        }
    }
}
