using FileManager;
using Xunit;

namespace UnitTest
{
    public class FileManagerTests
    {
        [Fact]
        public void GetContent_Contain_Program_cs()
        {
            // Arrange
            var directory = new MyDirectory(@"../../../../");
            var flags = new string[] { };
            var expected = "Program.cs";
            
            // Act
            var actual = directory.GetContent(flags);

            // Assert
            Assert.Contains(expected, actual);
        }
        
        [Fact]
        public void GetContent_That_Doesnt_Contain()
        {
            // Arrange
            var directory = new MyDirectory(@"../../../../");
            var flags = new string[] { };
            var expected = "z1z2z3";
            
            // Act
            var actual = directory.GetContent(flags);

            // Assert
            Assert.DoesNotContain(expected, actual);
        }

        [Fact]
        public void GetContent_With_Flags()
        {
            // Arrange
            var directory = new MyDirectory(@"../../../../");
            var flags = new string[] { "t" };
            var expected = "Creation time:";
            
            // Act
            var actual = directory.GetContent(flags);

            // Assert
            Assert.Contains(expected, actual);
        }
        
        [Fact]
        public void GetContent_With_Flags_That_Doesnt_Contain()
        {
            // Arrange
            var directory = new MyDirectory(@"../../../../");
            var flags = new string[] { "t" };
            var expected = "z1z2z3z";
            
            // Act
            var actual = directory.GetContent(flags);

            // Assert
            Assert.DoesNotContain(expected, actual);
        }
        
        [Fact]
        public void ViewFile()
        {
            // Arrange
            var file = new MyFile(@"../../../../Program.cs");
            var expected = "using System;";
            
            // Act
            var actual = file.View(200);

            // Assert
            Assert.Contains(expected, actual);
        }
        
        [Fact]
        public void ViewFile_That_Doesnt_Contain()
        {
            // Arrange
            var file = new MyFile(@"../../../../Program.cs");
            var expected = "z1z2z3";
            
            // Act
            var actual = file.View(200);

            // Assert
            Assert.DoesNotContain(expected, actual);
        }
        
        [Fact]
        public void Find_SubString_In_File()
        {
            // Arrange
            var file = new MyFile(@"../../../../Program.cs");
            
            // Act
            var result = file.IsSubstrExistInFile("using System;");

            // Assert
            Assert.True(result);
        }

        [Fact] public void Find_SubString_In_File_That_Doesnt_Exist()
        {
            // Arrange
            var file = new MyFile(@"../../../../Program.csdd");
            
            // Act
            var result = file.IsSubstrExistInFile("using System;");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateFile()
        {
            // Arrange
            var directory = new MyDirectory(@"../../../../");

            // Act
            var result = directory.MakeFile("1.txt");
            directory.Delete("1.txt");

            // Assert
            Assert.True(result);
        }
        
        [Fact]
        public void CreateFile_That_Already_Exist()
        {
            // Arrange
            var directory = new MyDirectory(@"../../../../");

            // Act
            var result = directory.MakeFile("Program.cs");

            // Assert
            Assert.False(result);
        }
    }
}
