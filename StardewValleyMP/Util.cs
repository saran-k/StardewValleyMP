﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System.IO.Compression;

namespace StardewValleyMP
{
    class Util
    {
        public static void drawStr(string str, float x, float y, Color col, float alpha = 1)
        {
            /*SpriteBatch b = Game1.spriteBatch;
            
            b.DrawString(Game1.smallFont, str, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(2f, 2f), Game1.textShadowColor * alpha);
            b.DrawString(Game1.smallFont, str, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(0f, 2f), Game1.textShadowColor * alpha);
            b.DrawString(Game1.smallFont, str, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(2f, 0f), Game1.textShadowColor * alpha);
            b.DrawString(Game1.smallFont, str, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)), col * 0.9f * alpha);*/
            SpriteBatch b = Game1.spriteBatch;
            Color inverted = new Color(255 - col.R, 255 - col.G, 255 - col.B);

            b.DrawString(Game1.smallFont, str, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(-2f, 0f), inverted * alpha * 0.8f);
            b.DrawString(Game1.smallFont, str, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(2f, 0f), inverted * alpha * 0.8f);
            b.DrawString(Game1.smallFont, str, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(0f, 2f), inverted * alpha * 0.8f);
            b.DrawString(Game1.smallFont, str, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(0f, -2f), inverted * alpha * 0.8f);
            b.DrawString(Game1.smallFont, str, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)), col * 0.9f * alpha);
        }

        // http://stackoverflow.com/a/17546909
        public static bool stringDialog( string title, ref string input )
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return ( result == DialogResult.OK );
        }
        
        public static bool yesNoDialog( string title, string text )
        {
            return (MessageBox.Show(title, text, MessageBoxButtons.YesNo) == DialogResult.Yes);
        }

        // http://stackoverflow.com/a/22456034
        public static string serialize< T >( T obj )
        {
            using ( MemoryStream stream = new MemoryStream() )
            {
                XmlSerializer serializer = new XmlSerializer( obj.GetType() );
                serializer.Serialize(stream, obj);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static T deserialize< T >( string str )
        {
            int beg = str.IndexOf('<');
            //string root = str.Substring(beg + 1, str.IndexOf(" xmlns") - beg - 1);
            XmlSerializer serializer = new XmlSerializer(typeof(T)/*, new XmlRootAttribute( root )*/);

            using ( TextReader reader = new StringReader( str ) )
            {
                return ( T )serializer.Deserialize(reader);
            }
        }

        // http://stackoverflow.com/questions/1879395/how-to-generate-a-stream-from-a-string
        public static Stream stringStream( string str )
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        // http://stackoverflow.com/questions/3303126/how-to-get-the-value-of-private-field-in-c
        public static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }

        public static void SetInstanceField(Type type, object instance, string fieldName, object value)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            field.SetValue(instance, value);
        }

        public static object GetStaticField(Type type, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(null);
        }

        public static void SetStaticField(Type type, string fieldName, object value)
        {
            BindingFlags bindFlags = BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            field.SetValue(null, value);
        }

        public static void CallStaticMethod(Type type, string name, object[] args)
        {
            // TODO: Support method overloading
            BindingFlags bindFlags = BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            MethodInfo func = type.GetMethod(name, bindFlags);
            func.Invoke(null, args);
        }

        public static void CallInstanceMethod(Type type, object instance, string name, object[] args)
        {
            // TODO: Support method overloading
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            MethodInfo func = type.GetMethod(name, bindFlags);
            func.Invoke(instance, args);
        }

        public static bool AreEqual< TKey, TVal >( Dictionary< TKey, TVal > a, Dictionary< TKey, TVal > b,
                                                   Func< TVal, TVal, bool > comp = null )
        {
            if (a.Count != b.Count)
                return false;
            else
            {
                List<TKey> keys = a.Keys.ToList();
                foreach (TKey key in b.Keys)
                {
                    if (!keys.Contains(key))
                    {
                        // A key is in the new but not old.
                        return false;
                    }
                    else if ( ( comp == null && !a[ key ].Equals( b[ key ] ) ) || ( comp != null && !comp( a[ key ], b[ key ] ) ) )
                    {
                        return false;
                    }
                    else
                    {
                        keys.Remove(key);
                    }
                }

                // A key was in the old but not the new.
                if (keys.Count > 0)
                    return false;
            }

            return true;
        }

        // Compression utils:
        /// <summary>
        /// Compresses byte array to new byte array.
        /// </summary>
        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionLevel.Optimal))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        public static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }
}
