using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrattParser.Test
{

    /// <summary>
    /// Test of the Lexer class.
    /// </summary>
    [TestClass]
    public class LexerEnumeratorTest
    {

        /// <summary>
        /// This class encapsulates IEO for an input string and an array of expected output TokenTypes.
        /// </summary>
        private class IEO_StringToTokenType : InputExpectedOutput<string, TokenType[]>
        {
            public IEO_StringToTokenType (string _input, TokenType[] _expectedOutput) : base(_input, _expectedOutput) { }
        }

        /// <summary>
        /// Array of IEO values for checking correct token type identification.
        /// </summary>
        private static IEO_StringToTokenType[] lexerList = new []
        {
            new IEO_StringToTokenType(" ",new [] { TokenType.EOF }),
            new IEO_StringToTokenType("+",new [] { TokenType.PLUS, TokenType.EOF }),
            new IEO_StringToTokenType("-",new [] { TokenType.MINUS, TokenType.EOF }),
            new IEO_StringToTokenType("/",new [] { TokenType.SLASH, TokenType.EOF }),
            new IEO_StringToTokenType("*",new [] { TokenType.ASTERISK, TokenType.EOF }),
            new IEO_StringToTokenType("^",new [] { TokenType.CARET, TokenType.EOF }),
            new IEO_StringToTokenType("if",new [] { TokenType.IF, TokenType.EOF }),
            new IEO_StringToTokenType("then",new [] { TokenType.THEN, TokenType.EOF }),
            new IEO_StringToTokenType("else",new [] { TokenType.ELSE, TokenType.EOF }),
            new IEO_StringToTokenType("==",new [] { TokenType.EQUAL, TokenType.EOF }),
            new IEO_StringToTokenType("<",new [] { TokenType.LESS_THAN, TokenType.EOF }),
            new IEO_StringToTokenType("<=",new [] { TokenType.LESS_THAN_OR_EQUAL, TokenType.EOF }),
            new IEO_StringToTokenType(">",new [] { TokenType.GREATER_THAN, TokenType.EOF }),
            new IEO_StringToTokenType(">=",new [] { TokenType.GREATER_THAN_OR_EQUAL, TokenType.EOF }),
            new IEO_StringToTokenType("!=",new [] { TokenType.NOT_EQUAL, TokenType.EOF }),
            new IEO_StringToTokenType("(",new [] { TokenType.LEFT_PAREN, TokenType.EOF }),
            new IEO_StringToTokenType(")",new [] { TokenType.RIGHT_PAREN, TokenType.EOF }),
            new IEO_StringToTokenType("42",new [] { TokenType.CONST_INT, TokenType.EOF }),
            new IEO_StringToTokenType("42.43",new [] { TokenType.CONST_DOUBLE, TokenType.EOF }),
            new IEO_StringToTokenType("prattparser",new [] { TokenType.IDENTIFIER, TokenType.EOF }),
            new IEO_StringToTokenType("if(abc==42)then",new [] { TokenType.IF, TokenType.LEFT_PAREN, TokenType.IDENTIFIER, TokenType.EQUAL,TokenType.CONST_INT,TokenType.RIGHT_PAREN,TokenType.THEN, TokenType.EOF }),
            new IEO_StringToTokenType("if(abc====42)then",new [] { TokenType.IF, TokenType.LEFT_PAREN, TokenType.IDENTIFIER, TokenType.EQUAL, TokenType.EQUAL,TokenType.CONST_INT,TokenType.RIGHT_PAREN,TokenType.THEN, TokenType.EOF }),
            new IEO_StringToTokenType("   if ( abc  == 42 )    then   ",new [] { TokenType.IF, TokenType.LEFT_PAREN, TokenType.IDENTIFIER, TokenType.EQUAL,TokenType.CONST_INT,TokenType.RIGHT_PAREN,TokenType.THEN, TokenType.EOF }),
            new IEO_StringToTokenType("ifabc==42)then",new [] { TokenType.IDENTIFIER, TokenType.EQUAL,TokenType.CONST_INT,TokenType.RIGHT_PAREN,TokenType.THEN, TokenType.EOF }),
            new IEO_StringToTokenType("if(abc== 42.3 )then",new [] { TokenType.IF, TokenType.LEFT_PAREN, TokenType.IDENTIFIER, TokenType.EQUAL,TokenType.CONST_DOUBLE,TokenType.RIGHT_PAREN,TokenType.THEN, TokenType.EOF })
        };

        /// <summary>
        /// Tests for correct token type identification in the IEO_StringToTokenType array.
        /// </summary>
        [TestMethod]
        public void TokenTypeTest()
        {
            Lexer lexer;

            // Loop over the IEO test values.
            foreach(IEO_StringToTokenType IEO in lexerList)
            {
                lexer = new Lexer(IEO.Input);
                
                // Retrieve enumerator for the tokens of the lexer. We need to call
                // MoveNext to move to the first element of the list.
                IEnumerator<Token> lexerEnumerator = lexer.GetEnumerator();

                lexerEnumerator.MoveNext();

                // Compare the token types in the expected output to the actual token types in tokenEnumerator.
                foreach (TokenType tokenType in IEO.ExpectedOutput)
                {
                    Assert.AreEqual(tokenType, lexerEnumerator.Current.TypeOfToken,
                        String.Format("Failed test on string {0}. Expected token type {1}, actual token type {2}", IEO.Input, tokenType, lexerEnumerator.Current.TypeOfToken));
                    lexerEnumerator.MoveNext();
                }
            }
        }

        /// <summary>
        /// Tests for correct functionality of the Lexer class in foreach statements.
        /// </summary>
        [TestMethod]
        public void ForeachTest()
        {
            int expected = 9;
            int actual;
            Lexer lexer = new Lexer("if (a == 0) then b");

            // Count the number of tokens up to and including the first EOF token in the lexer.
            actual = 0;
            foreach (Token token in lexer)
            {
                actual++;
                if (token.TypeOfToken == TokenType.EOF)
                {
                    break;
                }
            }

            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// Array of IEO values for checking exception throwing when failing token identification.
        /// </summary>
        private static string[] badStrings = new []
        {
            "¤", "if (a =! b)", "%!"
        };


        [TestMethod]
        public void BadTokenTest()
        {
            Lexer lexer;
            bool hasReachedEOF = false;

            foreach (string str in badStrings)
            {
                lexer = new Lexer(str);
                try
                {
                    foreach (Token token in lexer)
                    {
                        if (token.TypeOfToken == TokenType.EOF)
                        {
                            // If we get to this point, the enumerator of the lexer has failed to
                            // throw an exception before reaching an EOF token.
                            hasReachedEOF = true;
                            break;
                        }
                    }
                }
                catch
                {
                    // If we get to this point, the enumerator of the lexer has correctly thrown
                    // an exception before an EOF token was reached.
                }
            }

            if (hasReachedEOF)
            {
                Assert.Fail("Reached EOF token in spite of unidentifiable tokens");
            }
        }


    }
}
