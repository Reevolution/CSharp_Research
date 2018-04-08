using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prototype.System.Collections.Concurrent;

namespace Prototype.System.CollectionsTests.Concurrent
{
    [TestClass]
    public class BlockingQueueTest
    {
        [TestCategory("BlockingQueue")]
        [TestMethod]
        public void BlockingQueue_Enqueue_Collection_Size_is_Increased_When_Item_Enqueued_to_Collection()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            var startCount = blockingQueue.Count;

            //Act
            blockingQueue.Enqueue(42);

            //Assert
            Assert.AreEqual(0, startCount);
            Assert.AreEqual(1, blockingQueue.Count);
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        public void BlockingQueue_ToArray_Convert_Collection_to_Array_When_Collection_is_not_Empty()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            blockingQueue.Enqueue(1);
            blockingQueue.Enqueue(2);
            blockingQueue.Enqueue(3);

            //Act
            var array = blockingQueue.ToArray();

            //Assert
            Assert.AreEqual(blockingQueue.Count, array.Length);
            int index = 0;
            foreach (var n in blockingQueue)
            {
                Assert.AreEqual(n, array[index]);
                index++;
            }
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void BlockingQueue_Get_Count_Collection_Throw_ObjectDisposedException_When_Collection_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            blockingQueue.Dispose();

            //Act
            var count = blockingQueue.Count;

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void
            BlockingQueue_ToArray_Convert_Collection_to_Array_Throw_ObjectDisposedException_When_Collection_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            blockingQueue.Dispose();

            //Act
            var array = blockingQueue.ToArray();

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [DataTestMethod]
        [DataRow(-100)]
        [DataRow(-200)]
        [DataRow(-300)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BlockingQueue_TryPeek_Collection_Throw_ArgumentOutOfRangeException_When_Argument_is_Invalid(
            int value)
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            blockingQueue.Enqueue(value);

            //Act
            blockingQueue.TryPeek(out int item, value);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [DataTestMethod]
        [DataRow(-100)]
        [DataRow(-200)]
        [DataRow(-300)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BlockingQueue_TryDequeue_Collection_Throw_ArgumentOutOfRangeException_When_Argument_is_Invalid(
            int value)
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            blockingQueue.Enqueue(value);

            //Act
            blockingQueue.TryDequeue(out int item, value);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [DataTestMethod]
        [DataRow(42)]
        [DataRow(13)]
        [DataRow(66)]
        [DataRow(99)]
        public void BlockingQueue_TryDequeue_Collection_Return_Value_When_Collection_is_not_Empty(int value)
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            blockingQueue.Enqueue(value);

            //Act
            blockingQueue.TryDequeue(out int item);

            //Assert
            Assert.AreEqual(value, item);
        }

        [TestCategory("BlockingQueue")]
        [DataTestMethod]
        [DataRow(42)]
        [DataRow(13)]
        [DataRow(66)]
        [DataRow(99)]
        public void BlockingQueue_TryPeek_Collection_Return_Value_When_Collection_is_not_Empty(int value)
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            blockingQueue.Enqueue(value);

            //Act
            blockingQueue.TryPeek(out int item);

            //Assert
            Assert.AreEqual(value, item);
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        public void BlockingQueue_TryDequeue_Collection_Return_false_When_Collection_is_Empty()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            var state = blockingQueue.TryDequeue(out int item);

            //Assert
            Assert.AreEqual(false, state);
        }

        [TestCategory("BlockingQueue")]
        [DataTestMethod]
        [DataRow(42)]
        [DataRow(13)]
        [DataRow(66)]
        [DataRow(99)]
        public void BlockingQueue_TryPeek_Collection_Return_Item_by_TimeSpan(int value)
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                blockingQueue.Enqueue(value);
            });

            //Act
            blockingQueue.TryPeek(out int item, TimeSpan.FromMilliseconds(5000));

            //Assert
            Assert.AreEqual(value, item);
            Assert.AreEqual(1, blockingQueue.Count);
        }

        [TestCategory("BlockingQueue")]
        [DataTestMethod]
        [DataRow(42)]
        [DataRow(13)]
        [DataRow(66)]
        [DataRow(99)]
        public void BlockingQueue_TryPeek_Collection_Return_Item_by_Milliseconds(int value)
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                blockingQueue.Enqueue(value);
            });

            //Act
            blockingQueue.TryPeek(out int item, 5000);

            //Assert
            Assert.AreEqual(value, item);
            Assert.AreEqual(1, blockingQueue.Count);
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(OperationCanceledException))]
        public void BlockingQueue_TryPeek_is_Cancelled_by_CancellationToken()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            var cancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                cancellationTokenSource.Cancel();
            });

            //Act
            blockingQueue.TryPeek(out int item, 5000, cancellationTokenSource.Token);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        public void BlockingQueue_TryPeek_return_False_by_Timeout()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            var cancellationTokenSource = new CancellationTokenSource();

            //Act
            var state = blockingQueue.TryPeek(out int item, 500, cancellationTokenSource.Token);

            //Assert 
            Assert.IsFalse(state);
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        public void BlockingQueue_TryPeek_Collection_Return_false_When_Collection_is_Empty()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            var state = blockingQueue.TryPeek(out int item);

            //Assert
            Assert.AreEqual(false, state);
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        public void BlockingQueue_TryDequeue_Collection_Size_is_Decreased_When_Item_Dequeued_from_Collection()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            blockingQueue.Enqueue(42);
            var startCount = blockingQueue.Count;

            //Act
            blockingQueue.TryDequeue(out int item);

            //Assert
            Assert.AreEqual(1, startCount);
            Assert.AreEqual(0, blockingQueue.Count);
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        public void BlockingQueue_TryPeek_Collection_Size_is_not_Changed_When_Item_Peeked_from_Collection()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            blockingQueue.Enqueue(42);
            var startCount = blockingQueue.Count;

            //Act
            blockingQueue.TryPeek(out int item);

            //Assert
            Assert.AreEqual(1, startCount);
            Assert.AreEqual(1, blockingQueue.Count);
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void BlockingQueue_Enqueue_Throw_ObjectDisposedException_When_BlockingQueue_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            blockingQueue.Dispose();
            blockingQueue.Enqueue(22);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [DataTestMethod]
        [DataRow(42)]
        [DataRow(13)]
        [DataRow(66)]
        [DataRow(99)]
        public void BlockingQueue_TryDequeue_Decrease_Collection_Size_and_Return_Item_by_TimeSpan(int value)
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                blockingQueue.Enqueue(value);
            });

            //Act
            blockingQueue.TryDequeue(out int item, TimeSpan.FromMilliseconds(5000));

            //Assert
            Assert.AreEqual(value, item);
            Assert.AreEqual(0, blockingQueue.Count);
        }

        [TestCategory("BlockingQueue")]
        [DataTestMethod]
        [DataRow(42)]
        [DataRow(13)]
        [DataRow(66)]
        [DataRow(99)]
        public void BlockingQueue_TryDequeue_Decrease_Collection_Size_and_Return_Item_by_Milliseconds(int value)
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                blockingQueue.Enqueue(value);
            });

            //Act
            blockingQueue.TryDequeue(out int item, 5000);

            //Assert
            Assert.AreEqual(value, item);
            Assert.AreEqual(0, blockingQueue.Count);
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(OperationCanceledException))]
        public void BlockingQueue_TryDequeue_is_Cancelled_by_CancellationToken()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            var cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                cancellationTokenSource.Cancel();
            });

            //Act
            blockingQueue.TryDequeue(out int item, 5000, cancellationTokenSource.Token);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        public void BlockingQueue_TryDequeue_return_False_by_Timeout()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            var cancellationTokenSource = new CancellationTokenSource();

            //Act
            var state = blockingQueue.TryDequeue(out int item, 500, cancellationTokenSource.Token);

            //Assert 
            Assert.IsFalse(state);
            Assert.AreEqual(0, blockingQueue.Count);
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void BlockingQueue_TryDequeue_Throw_ObjectDisposedException_When_BlockingQueue_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            blockingQueue.Dispose();
            blockingQueue.TryDequeue(out int item);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void
            BlockingQueue_TryDequeue_Throw_ObjectDisposedException_With_TimeSpan_When_BlockingQueue_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            blockingQueue.Dispose();
            blockingQueue.TryDequeue(out int item, new TimeSpan(0));

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void
            BlockingQueue_TryDequeue_Throw_ObjectDisposedException_With_Milliseconds_When_BlockingQueue_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            blockingQueue.Dispose();
            blockingQueue.TryDequeue(out int item, 0);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void
            BlockingQueue_TryDequeue_Throw_ObjectDisposedException_With_Milliseconds_and_CancellationToken_When_BlockingQueue_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            blockingQueue.Dispose();
            blockingQueue.TryDequeue(out int item, 0, CancellationToken.None);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void BlockingQueue_TryPeek_Throw_ObjectDisposedException_When_BlockingQueue_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            blockingQueue.Dispose();
            blockingQueue.TryPeek(out int item);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void BlockingQueue_TryPeek_Throw_ObjectDisposedException_With_TimeSpan_When_BlockingQueue_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            blockingQueue.Dispose();
            blockingQueue.TryPeek(out int item, new TimeSpan(0));

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void
            BlockingQueue_TryPeek_Throw_ObjectDisposedException_With_Milliseconds_When_BlockingQueue_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            blockingQueue.Dispose();
            blockingQueue.TryPeek(out int item, 0);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void
            BlockingQueue_TryPeek_Throw_ObjectDisposedException_With_Milliseconds_and_CancellationToken_When_BlockingQueue_is_Disposed()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();

            //Act
            blockingQueue.Dispose();
            blockingQueue.TryPeek(out int item, 0, CancellationToken.None);

            //Assert is handled by the ExpectedException
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        public void BlockingQueue_Should_Serialize_and_Deserialize_by_Binaryformatter_and_Should_be_the_same()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            blockingQueue.Enqueue(1);
            blockingQueue.Enqueue(2);
            blockingQueue.Enqueue(3);

            //Act
            byte[] blockingQueueSerialized;
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, blockingQueue);
                blockingQueueSerialized = ms.GetBuffer();
            }

            BlockingQueue<int> blockingQueueDeSerialized;
            using (var ms = new MemoryStream(blockingQueueSerialized))
            {
                var bf = new BinaryFormatter();
                blockingQueueDeSerialized = (BlockingQueue<int>) bf.Deserialize(ms);
            }

            //Assert
            Assert.IsTrue(blockingQueue.SequenceEqual(blockingQueueDeSerialized));
        }

        [TestCategory("BlockingQueue")]
        [TestMethod]
        public void BlockingQueue_Represent_a_Queue()
        {
            //Arrange
            var blockingQueue = new BlockingQueue<int>();
            int[] array = new[] {0, 1, 2, 3, 4, 6, 7, 8, 9};

            //Act
            foreach (var n in array)
            {
                blockingQueue.Enqueue(n);
            }

            //Assert
            Assert.IsTrue(blockingQueue.SequenceEqual(array));
        }
    }
}
