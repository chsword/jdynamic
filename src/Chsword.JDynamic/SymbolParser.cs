using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Chsword
{
    /// <summary>
    /// Symbol Parser
    /// </summary>
    [DebuggerStepThrough]
    [DebuggerDisplay("CurrentPosition = {CurrentPosition}, Source = {Source}")]
    internal sealed class SymbolParser
    {
        #region Fields And Properties
        /// <summary>
        /// Gets the source.
        /// </summary>
        public string Source { get; private set; }
        /// <summary>
        /// Gets the current position.
        /// </summary>
        public int CurrentPosition { get; private set; }
        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length { get; private set; }
        /// <summary>
        /// Gets the current char.
        /// </summary>
        public char CurrentChar { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolParser"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public SymbolParser(string source)
        {
            if (ReferenceEquals(null, source))
                throw new ArgumentNullException("source");

            Source = source;
            Length = source.Length;
            SetPosition(0);
        }
        #endregion

        #region Business Methods
        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="index">The index.</param>
        public void SetPosition(int index)
        {
            CurrentPosition = index;
            CurrentChar = CurrentPosition < Length ? Source[CurrentPosition] : '\0';
        }

        /// <summary>
        /// Nexts the char.
        /// </summary>
        public void NextChar()
        {
            if (CurrentPosition < Length) CurrentPosition++;
            CurrentChar = CurrentPosition < Length ? Source[CurrentPosition] : '\0';
        }

        /// <summary>
        /// Nexts the token.
        /// </summary>
        /// <returns></returns>
        public string NextToken()
        {
            while (Char.IsWhiteSpace(CurrentChar)) NextChar();
            int tokenPos = CurrentPosition;
            switch (CurrentChar)
            {
                case '[':
                case ']':
                case '{':
                case '}':
                case ':':
                case ',':
                    NextChar();
                    break;
                case '"':
                case '\'':
                    char quote = CurrentChar;
                    do
                    {
                        NextChar();
                        while (CurrentPosition < Length && CurrentChar != quote) NextChar();
                        if (CurrentPosition == Length)
                            throw ParseError(CurrentPosition, "Unterminated string literal");
                        NextChar();
                    } while (CurrentChar == quote);
                    break;
                default:
                    if (Char.IsLetter(CurrentChar) || CurrentChar == '_')
                    {
                        do
                        {
                            NextChar();
                        } while (Char.IsLetterOrDigit(CurrentChar) || CurrentChar == '_' || CurrentChar == '?');
                        break;
                    }
                    if (Char.IsDigit(CurrentChar)|| CurrentChar=='-')
                    {
                        do
                        {
                            NextChar();
                        } while (Char.IsDigit(CurrentChar));

                        if (CurrentChar == 'l' || CurrentChar == 'L')
                        {
                            NextChar();
                            break;
                        }
                        else if (CurrentChar == 'f' || CurrentChar == 'F')
                        {
                            NextChar();
                            break;
                        }
                        else if (CurrentChar == 'm' || CurrentChar == 'M')
                        {
                            NextChar();
                            break;
                        }
                        else if (CurrentChar == 'd' || CurrentChar == 'D')
                        {
                            NextChar();
                            break;
                        }

                        if (CurrentChar == '.')
                        {
                            NextChar();
                            ValidateDigit();
                            do
                            {
                                NextChar();
                            } while (Char.IsDigit(CurrentChar));
                        }

                        if (CurrentChar == 'E' || CurrentChar == 'e')
                        {
                            NextChar();
                            if (CurrentChar == '+' || CurrentChar == '-') NextChar();
                            ValidateDigit();
                            do
                            {
                                NextChar();
                            } while (Char.IsDigit(CurrentChar));
                        }

                        if (CurrentChar == 'F' || CurrentChar == 'f')
                        {
                            NextChar();
                            break;
                        }
                        else if (CurrentChar == 'm' || CurrentChar == 'M')
                        {
                            NextChar();
                            break;
                        }
                        else if (CurrentChar == 'd' || CurrentChar == 'D')
                        {
                            NextChar();
                            break;
                        }

                        break;
                    }
                    if (CurrentPosition == Length)
                        break;
                    throw ParseError(CurrentPosition, "Syntax error '{0}'", CurrentChar);
            }
            var symbol = Source.Substring(tokenPos, CurrentPosition - tokenPos);
            return symbol;
        }

        /// <summary>
        /// Builds the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The Build result.</returns>
        public static SymbolParseResult Build(string source)
        {
            var item = new SymbolParser(source);
            List<KeyValuePair<int, string>> data = new List<KeyValuePair<int, string>>();
            while (true)
            {
                var index = item.CurrentPosition;
                var symbol = item.NextToken();
                if (string.IsNullOrWhiteSpace(symbol))
                    break;
                data.Add(new KeyValuePair<int, string>(index, symbol));
            }
            return new SymbolParseResult(data);
        }
        #endregion

        #region Private Methods
        private void ValidateDigit()
        {
            if (!Char.IsDigit(CurrentChar)) throw ParseError(CurrentPosition, "Digit expected");
        }

        private Exception ParseError(int pos, string format, params object[] args)
        {
            var msg = string.Format(CultureInfo.CurrentCulture, format, args);
            return new ApplicationException(string.Format("{0} (at index {1})", msg, pos));
        }
        #endregion
    }
}
