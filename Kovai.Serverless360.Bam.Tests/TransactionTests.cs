#region Using Directives
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
#endregion

namespace Kovai.Serverless360.Bam.Tests
{
    public class TransactionTests
    {
        private const string Key = "provide your key";
        public TransactionTests()
        {

        }

        [Fact]
        public void TestStartTransaction()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(new StartTransactionResponse()
                    {
                        TransactionInstanceId = Guid.NewGuid(),
                        StageInstanceId = Guid.NewGuid()
                    }.Serialize())
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/")
            };

            ITransactionService transactionService = new TransactionService(Key, httpClient);

            var result = transactionService
                .StartTransaction(
                    new StartTransactionRequest
                    {
                        BusinessProcess = "Test",
                        Transaction = "Test",
                        Stage = "Test",
                        MessageBody = "Test",
                        MessageHeader = "{\"some\":1}"
                    }).Result;

            Assert.NotNull(result);
        }

        [Theory]

        [InlineData("", "Test", "Test", "Test", "Test")]
        [InlineData("Test", "", "Test", "Test", "Test")]
        [InlineData("Test", "Test", "", "Test", "Test")]
        [InlineData("Test", "Test", "Test", "", "Test")]
        [InlineData("Test", "Test", "Test", "Test", "")]
        public void TestNegativeScenarios(string businessProcess, string businessTransaction, string currentStage, string messageBody, string messageHeader)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(new StartTransactionResponse()
                    {
                        TransactionInstanceId = Guid.NewGuid(),
                        StageInstanceId = Guid.NewGuid()
                    }.Serialize())
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/")
            };

            ITransactionService transactionService = new TransactionService(Key, httpClient);
            var result = transactionService
                .StartTransaction(new StartTransactionRequest()
                {
                    BusinessProcess = businessProcess,
                    Transaction = businessTransaction,
                    Stage = currentStage,
                    MessageBody = messageBody,
                    MessageHeader = messageHeader
                }).Result;

            Assert.True(result.StageInstanceId == default);
        }

        [Fact]
        public void TestCheckPoint()
        {
            //Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/")
            };

            ITransactionService transactionService = new TransactionService(Key, httpClient);

            //Act
            var response = transactionService.CheckPoint(new CheckPointRequest()
            {
                MessageBody = "Test",
                MessageHeader = "{\"some\":1}",
                TransactionInstanceId = Guid.NewGuid(),
                StageInstanceId = Guid.NewGuid(),
                Stage = "Test",
                StageStatus = StageStatus.Success
            }).Result;

            //Assert
            Assert.True(response);
        }

        [Fact]
        public void TestCorrelationCheckPoint()
        {
            //Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/")
            };

            ITransactionService transactionService = new TransactionService(Key, httpClient);

            //Act
            var response = transactionService.CorrelationCheckPoint(new CorrelationCheckPointRequest()
            {
                CorrelationProperties = new System.Collections.Generic.Dictionary<string, object>(),
                StageStatus = StageStatus.Success,
                Stage = "Test",
                MessageBody = "Test",
                MessageHeader = "{\"some\":1}",
                StageInstanceId = Guid.NewGuid()
            }).Result;

            //Assert
            Assert.True(response);
        }
    }
}
