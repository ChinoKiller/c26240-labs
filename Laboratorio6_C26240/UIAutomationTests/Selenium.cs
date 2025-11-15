using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace UIAutomationTests
{
    public class Selenium
    {
        // ====================== Entregable primera parte ======================
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new FirefoxDriver();
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Quit();
            _driver.Dispose();
            Console.WriteLine($"El test \"{TestContext.CurrentContext.Test.Name}\" se completo correctamente");
        }

        [Test]
        [Order(1)]
        public void Enter_To_List_Of_Countries_Test()
        {
            // Arrange
            var URL = "http://localhost:8080/";

            // Maximiza
            _driver.Manage().Window.Maximize();

            // Act
            _driver.Navigate().GoToUrl(URL);

            // Assert
            Assert.That(_driver, Is.Not.Null);
        }

        // ====================== Entregable segunda parte ======================
        [Test]
        [Order(2)]
        public void Create_Country_Test()
        {
            // Arrange
            string URL = "http://localhost:8080/";
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(URL);

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // 1. Ir al boton de agregar pais
            var addButton = wait.Until(d =>
                d.FindElement(By.XPath("//button[contains(text(), 'Agregar país')]"))
            );
            addButton.Click();

            // 2. Llenar el formulario
            wait.Until(d => d.FindElement(By.Id("name"))).SendKeys("Italia");
            _driver.FindElement(By.Id("continent")).SendKeys("Europa");
            _driver.FindElement(By.Id("language")).SendKeys("Italiano");

            // 3. Guardar
            var saveButton = _driver.FindElement(By.XPath("//button[contains(text(), 'Guardar')]"));
            saveButton.Click();

            // 4. Confirmacion: esperar el modal de pais creado correctamente.
            var successMsg = wait.Until(d =>
                d.FindElement(By.XPath("//*[contains(text(),'País creado correctamente')]"))
            );

            // 5. Assert real
            Assert.That(successMsg.Text, Does.Contain("correctamente"));
        }

        // ====================== Extra: verificacion ======================
        [Test]
        [Order(3)]
        public void Verify_New_Country_In_Table_Test()
        {
            // Arrange
            string URL = "http://localhost:8080/";
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(URL);

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Esperar que cargue la tabla
            wait.Until(driver => driver.FindElement(By.TagName("table")));

            // Buscar fila que contenga el pais Italia
            var row = wait.Until(driver =>
                driver.FindElement(By.XPath("//tr[td[contains(text(),'Italia')]]"))
            );

            // Validar columnas especificas dentro de la fila
            var columns = row.FindElements(By.TagName("td"));

            string nombre = columns[0].Text;
            string continente = columns[1].Text;
            string idioma = columns[2].Text;

            // Asserts
            Assert.That(nombre, Is.EqualTo("Italia"));
            Assert.That(continente, Is.EqualTo("Europa"));
            Assert.That(idioma, Is.EqualTo("Italiano"));
        }
    }
}





