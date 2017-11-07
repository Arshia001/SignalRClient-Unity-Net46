/*
 * Created by Arshia001
 * Based heavily on http://wiki.unity3d.com/index.php/SimpleJSON by Bunny83
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotSoSimpleJSON
{
    public abstract class JSONNode
    {
        public bool IsArray { get { return Type == JSONNodeType.Array; } }

        public bool IsObject { get { return Type == JSONNodeType.Object; } }

        public bool IsNull { get { return Type == JSONNodeType.NullValue; } }

        public virtual JSONNode this[int aIndex] { get { return new JSONNonexistent(); } set { throw new NotSupportedException(); } }

        public virtual JSONNode this[string aKey] { get { return new JSONNonexistent(); } set { throw new NotSupportedException(); } }

        public virtual int Count { get { return -1; } }

        public virtual IEnumerable<JSONNode> Children { get { throw new NotSupportedException(); } }

        public virtual void Add(JSONNode aItem)
        {
            throw new NotSupportedException();
        }

        public virtual void Add(string aKey, JSONNode aItem)
        {
            throw new NotSupportedException();
        }

        public virtual JSONNode Remove(string aKey)
        {
            throw new NotSupportedException();
        }

        public virtual JSONNode Remove(int aIndex)
        {
            throw new NotSupportedException();
        }

        public virtual JSONNode Remove(JSONNode aNode)
        {
            throw new NotSupportedException();
        }

        protected abstract void WriteToStringBuilder(StringBuilder StringBuilder);

        public string Serialize()
        {
            var Result = new StringBuilder();
            WriteToStringBuilder(Result);
            return Result.ToString();
        }

        public abstract JSONNodeType Type { get; }

        public virtual int? AsInt
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public virtual double? AsDouble
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public virtual bool? AsBool
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public virtual JSONArray AsArray
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public virtual JSONObject AsObject
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public virtual string AsString
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public virtual Dictionary<string, JSONNode> AsDictionary
        {
            get
            {
                throw new NotSupportedException();
            }
        }


        public override bool Equals(object obj)
        {
            return this == obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(JSONNode N, object O)
        {
            if (O == null && (N is JSONNull || N is JSONNonexistent))
                return true;
            else if ((O is int || O is int?) && N is JSONInt)
                return N.AsInt == (int)O;
            else if ((O is float || O is float? || O is double || O is double?) && N is JSONDouble)
                return N.AsDouble == (double)O;
            else if ((O is bool || O is bool?) && N is JSONBool)
                return N.AsBool == (bool)O;
            else if (O is string && N is JSONString)
                return N.AsString == (string)O;
            else
                return ReferenceEquals(N, O);
        }

        public static bool operator !=(JSONNode N, object O)
        {
            return !(N == O);
        }

        public static implicit operator JSONNode(string s)
        {
            return new JSONString(s);
        }

        public static implicit operator JSONNode(int s)
        {
            return new JSONInt(s);
        }

        public static implicit operator JSONNode(float s)
        {
            return new JSONDouble(s);
        }

        public static implicit operator JSONNode(double s)
        {
            return new JSONDouble(s);
        }

        public static implicit operator JSONNode(bool s)
        {
            return new JSONBool(s);
        }


        internal static string Escape(string aText)
        {
            StringBuilder result = new StringBuilder(aText.Length);
            foreach (char c in aText)
            {
                switch (c)
                {
                    case '\\':
                        result.Append("\\\\");
                        break;
                    case '\"':
                        result.Append("\\\"");
                        break;
                    case '\n':
                        result.Append("\\n");
                        break;
                    case '\r':
                        result.Append("\\r");
                        break;
                    case '\t':
                        result.Append("\\t");
                        break;
                    case '\b':
                        result.Append("\\b");
                        break;
                    case '\f':
                        result.Append("\\f");
                        break;
                    default:
                        result.Append(c);
                        break;
                }
            }
            return result.ToString();
        }

        static JSONNode Numberize(string token)
        {
            bool flag = false;
            int integer = 0;
            double real = 0;

            if (int.TryParse(token, out integer))
            {
                return new JSONInt(integer);
            }

            if (double.TryParse(token, out real))
            {
                return new JSONDouble(real);
            }

            if (bool.TryParse(token, out flag))
            {
                return new JSONBool(flag);
            }

            throw new NotImplementedException(token);
        }

        static JSONNode GetElement(string token, bool tokenIsString)
        {
            if (tokenIsString)
            {
                return new JSONString(token);
            }
            else if (token == "null")
            {
                return new JSONNull();
            }
            else
            {
                return Numberize(token);
            }
        }

        static void AddElement(JSONNode ctx, string token, string tokenName, bool tokenIsString)
        {
            if (ctx.IsArray)
                ctx.Add(GetElement(token, tokenIsString));
            else if (ctx.IsObject)
                ctx.Add(tokenName, GetElement(token, tokenIsString));
            else
                throw new NotSupportedException();
        }

        public static JSONNode Parse(string aJSON)
        {
            Stack<JSONNode> stack = new Stack<JSONNode>();
            JSONNode ctx = null;
            int i = 0;
            string Token = "";
            string TokenName = "";
            bool QuoteMode = false;
            bool TokenIsString = false;
            while (i < aJSON.Length)
            {
                switch (aJSON[i])
                {
                    case '{':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                            break;
                        }
                        stack.Push(new JSONObject());
                        if (ctx != null)
                        {
                            TokenName = TokenName.Trim();
                            if (ctx is JSONArray)
                                ctx.Add(stack.Peek());
                            else if (TokenName != "")
                                ctx.Add(TokenName, stack.Peek());
                        }
                        TokenName = "";
                        Token = "";
                        ctx = stack.Peek();
                        break;

                    case '[':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                            break;
                        }

                        stack.Push(new JSONArray());
                        if (ctx != null)
                        {
                            TokenName = TokenName.Trim();

                            if (ctx is JSONArray)
                                ctx.Add(stack.Peek());
                            else if (TokenName != "")
                                ctx.Add(TokenName, stack.Peek());
                        }
                        TokenName = "";
                        Token = "";
                        ctx = stack.Peek();
                        break;

                    case '}':
                    case ']':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                            break;
                        }
                        if (stack.Count == 0)
                            throw new Exception("JSON Parse: Too many closing brackets");

                        stack.Pop();
                        if (Token != "")
                        {
                            TokenName = TokenName.Trim();
                            /*
							if (ctx is JSONArray)
								ctx.Add (Token);
							else if (TokenName != "")
								ctx.Add (TokenName, Token);
								*/
                            AddElement(ctx, Token, TokenName, TokenIsString);
                            TokenIsString = false;
                        }
                        TokenName = "";
                        Token = "";
                        if (stack.Count > 0)
                            ctx = stack.Peek();
                        break;

                    case ':':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                            break;
                        }
                        TokenName = Token;
                        Token = "";
                        TokenIsString = false;
                        break;

                    case '"':
                        QuoteMode ^= true;
                        TokenIsString = QuoteMode == true ? true : TokenIsString;
                        break;

                    case ',':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                            break;
                        }
                        if (Token != "")
                        {
                            /*
							if (ctx is JSONArray) {
								ctx.Add (Token);
							} else if (TokenName != "") {
								ctx.Add (TokenName, Token);
							}
							*/
                            AddElement(ctx, Token, TokenName, TokenIsString);
                            TokenIsString = false;

                        }
                        TokenName = "";
                        Token = "";
                        TokenIsString = false;
                        break;

                    case '\r':
                    case '\n':
                        break;

                    case ' ':
                    case '\t':
                        if (QuoteMode)
                            Token += aJSON[i];
                        break;

                    case '\\':
                        ++i;
                        if (QuoteMode)
                        {
                            char C = aJSON[i];
                            switch (C)
                            {
                                case 't':
                                    Token += '\t';
                                    break;
                                case 'r':
                                    Token += '\r';
                                    break;
                                case 'n':
                                    Token += '\n';
                                    break;
                                case 'b':
                                    Token += '\b';
                                    break;
                                case 'f':
                                    Token += '\f';
                                    break;
                                case 'u':
                                    {
                                        string s = aJSON.Substring(i + 1, 4);
                                        Token += (char)int.Parse(
                                            s,
                                            System.Globalization.NumberStyles.AllowHexSpecifier);
                                        i += 4;
                                        break;
                                    }
                                default:
                                    Token += C;
                                    break;
                            }
                        }
                        break;

                    default:
                        Token += aJSON[i];
                        break;
                }
                ++i;
            }
            if (QuoteMode)
            {
                throw new Exception("JSON Parse: Quotation marks seem to be messed up.");
            }

            if (ctx == null)
                return GetElement(Token, TokenIsString);

            return ctx;
        }

        public virtual bool IsEmpty
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}
