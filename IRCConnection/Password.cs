using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace IRCConnection
{
    public class Password: IPassword, IXmlSerializable
    {
        #region IPassword Members

        public string Nick { get; set; }

        public string Pass { get; set; }

        #endregion

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Прочитать пароль из XML
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            bool read = false;
            while (read || reader.Read())
            {
                read = false;
                if (reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Nick":
                            this.Nick = reader.ReadElementContentAsString();
                            read = true;
                            break;

                        case "Pass":
                            MemoryStream stream = null;
                            CryptoStream cryptoStream = null;

                            try
                            {
                                var crypt = CreateCipher();

                                var transform = crypt.CreateDecryptor();
                                var value = Convert.FromBase64String(reader.ReadElementContentAsString());
                                read = true;
                                stream = new MemoryStream(value);
                                var buf = new byte[1024];
                                try
                                {
                                    cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
                                    cryptoStream.Read(buf, 0, buf.Length);
                                }
                                finally
                                {
                                    cryptoStream.Close();
                                }
                                this.Pass = Encoding.ASCII.GetString(buf);
                                this.Pass = this.Pass.Substring(0, this.Pass.IndexOf('\0'));
                            }
                            catch (Exception exc)
                            {
                                Trace.WriteLine(exc.Message);
                            }
                            finally
                            {
                                stream.Close();
                            }
                            break;

                        default:
                            break;
                    }
                }
                else if (reader.NodeType == System.Xml.XmlNodeType.EndElement && reader.Name == "Password")
                {
                    reader.Read();
                    break;
                }
            }
        }

        /// <summary>
        /// Сериализоваться в Xml
        /// </summary>
        /// <param name="writer">Оболочка потока сериализации</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Nick");
            writer.WriteString(this.Nick);
            writer.WriteEndElement();
            writer.WriteStartElement("Pass");
            MemoryStream stream = null;
            CryptoStream cryptoStream = null;

            try
            {
                var crypt = CreateCipher();

                var transform = crypt.CreateEncryptor();
                stream = new MemoryStream();
                try
                {
                    cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                    byte[] value = Encoding.ASCII.GetBytes(this.Pass);
                    cryptoStream.Write(value, 0, value.Length);
                }
                finally
                {
                    cryptoStream.Close();
                }
                writer.WriteString(Convert.ToBase64String(stream.ToArray()));
                
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message);
            }
            finally
            {                
                stream.Close();
            }
            writer.WriteEndElement();
        }

        private static RijndaelManaged CreateCipher()
        {
            var crypt = new RijndaelManaged();

            byte[] salt = Encoding.ASCII.GetBytes(Assembly.GetEntryAssembly().GetName().Name + "kYuin8-90");
            var key = new Rfc2898DeriveBytes(Environment.UserName, salt);
            crypt.Key = key.GetBytes(crypt.KeySize / 8);
            crypt.IV = key.GetBytes(crypt.BlockSize / 8);
            return crypt;
        }

        #endregion
    }
}
