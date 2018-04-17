using System;
using Novell.Directory.Ldap;

namespace AspNetCore.LdapAuthentication
{
    /// <inheritdoc />
    /// <summary>
    /// A class that provides password verification against an LDAP store by attempting to bind.
    /// </summary>
    public class LdapAuthentication :  IDisposable
    {
        private bool _isDisposed;
        private readonly LdapConnection _connection;
        private readonly LdapAuthenticationOptions _options;
        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SamAccountNameAttribute = "sAMAccountName";


        /// <summary>
        /// Initializes a new instance with the the given options.
        /// </summary>
        /// <param name="options"></param>
        public LdapAuthentication(LdapAuthenticationOptions options)
        {
            _options = options;
            _connection = new LdapConnection();
        }

        /// <summary>
        /// Cleans up any connections and other resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }            

            _connection.Dispose();
            _isDisposed = true;
        }

        /// <summary>
        /// Gets a value that indicates if the password for the user identified by the given DN is valid.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidatePassword(string username, string password)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(LdapConnection));
            }

            if (string.IsNullOrEmpty(_options.Url))
            {
                throw new InvalidOperationException("The LDAP Hostname cannot be empty or null.");
            }

            _connection.Connect(_options.Url, _options.Port);
            _connection.Bind(_options.BindDn, _options.BindCredentials);

            var searchFilter = string.Format(_options.SearchFilter, username);
            var result = _connection.Search(
                _options.SearchBase,
                LdapConnection.SCOPE_SUB,
                searchFilter,
                new[] { MemberOfAttribute, DisplayNameAttribute, SamAccountNameAttribute },
                false
            );

            try
            {
                var user = result.next();
                if (user != null)
                {
                    return false;
                }

                _connection.Bind(username, password);

                return _connection.Bound;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                _connection.Disconnect();
            }
        }
    }

}
