using NameSortering;

namespace NameSortingTestCases
{

    [TestFixture]
        public class NameSorterAppTests
        {
            [Test]
            public void Run_ValidInputFile_OutputFileExists()
            {
                // Arrange

                string inputFilePath = "valid_input.txt";
                string outputFilePath = "output.txt";


                // Create a test input file
                File.WriteAllLines(inputFilePath, new[] {
                                                    "Janet Parsons","Vaugh Lewis","Adonis Julius Arche"," Shelby Nathan Yoder",
                                                   "Marin Alvarez",
                                                   "London Lindsey",
                                                   "Beau Tristan Bentley",
                                                   "Leo Gardner",
                                                   "Hunter Uriah Mathew Clarke",
                                                   "Mikayla Lopez",
                                                "Fankie Conner Ritter" });

                // Act
                var nameParser = new NameParser();
                var nameSorter = new NameSorter();
                var nameOutput = new NameOutput();
                var nameSorterApp = new NameSorterApp(nameParser, nameSorter, nameOutput);
                nameSorterApp.Run(inputFilePath, outputFilePath);

                // Assert
                Assert.That(File.Exists(outputFilePath), Is.True);

                // Clean up: Delete test files
                File.Delete(inputFilePath);
                File.Delete(outputFilePath);
            }

            [Test]
            public void Run_EmptyInputFile_NoOutputFileCreated()
            {
                // Arrange
                string inputFilePath = "empty_input.txt";
                string outputFilePath = "output.txt";

                // Create an empty input file
                File.WriteAllText(inputFilePath, "");

                // Act
                var nameParser = new NameParser();
                var nameSorter = new NameSorter();
                var nameOutput = new NameOutput();
                var nameSorterApp = new NameSorterApp(nameParser, nameSorter, nameOutput);
                nameSorterApp.Run(inputFilePath, outputFilePath);

                // Assert
                Assert.That(File.Exists(outputFilePath), Is.False);

                // Clean up: Delete test files
                File.Delete(inputFilePath);
            }
        }


    
}