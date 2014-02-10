using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PrattParser;

namespace PrattParser.Test
{

    [TestClass]
    public class ParserTest
    {
        public void printExpressionTree(Expression expr)
        {
            

        }

        [TestMethod]
        public void SimpleParserTest()
        {
            string parseString = "2+3*4";
            
            Lexer lexer = new Lexer(parseString);
            Grammar grammar = new Grammar();
            Parser parser = new Parser(lexer.GetEnumerator(), grammar);
            Expression expr = parser.parse();

            ExpressionPrinter writer = new ExpressionPrinter();
            writer.writeExpression(expr);
   
        }
    }
}
