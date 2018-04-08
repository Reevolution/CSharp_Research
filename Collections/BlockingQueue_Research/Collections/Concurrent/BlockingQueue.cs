using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace Prototype.System.Collections.Concurrent
{
    /// <summary>
    /// Provides blocking and bounding capabilities for thread-safe collections that implement <see cref="IProducerConsumerCollection{T}"/> and represents them like a thread-safe first in-first out (FIFO) collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    [Serializable]
    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
    public class BlockingQueue<T> : IEnumerable<T>, IEnumerable, IDisposable, IReadOnlyCollection<T>
    {
        private const int NON_BOUNDED = -1;

        private bool _isDisposed;
        private T[] _serializationArray;
        private ConcurrentQueue<T> _collection;
        [NonSerialized]
        private CancellationTokenSource _blockinQueueCancellationTokenSource;

        /// <inheritdoc />
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:Prototype.System.Collections.Concurrent.BlockingQueue`1" />.</summary>
        /// <returns>The number of elements contained in the <see cref="T:Prototype.System.Collections.Concurrent.BlockingQueue`1" />.</returns>
        public int Count
        {
            get
            {
                this.CheckDisposed();
                return _collection.Count;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of <see cref="T:Prototype.System.Collections.Concurrent.BlockingQueue`1" /> class.
        /// </summary>
        public BlockingQueue() : this(new ConcurrentQueue<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BlockingQueue{T}"/> class that contains elements copied from the specified collection, that implements <see cref="IProducerConsumerCollection{T}"/>.
        /// </summary>
        /// <param name="collection"></param>
        public BlockingQueue(IProducerConsumerCollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            this.Initialize(collection);
        }

        private void Initialize(IProducerConsumerCollection<T> collection)
        {
            _collection = new ConcurrentQueue<T>(collection);
            _blockinQueueCancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Adds an item to the end of the <see cref="BlockingQueue{T}"/>.
        /// </summary>
        /// <param name="item">The item to add to the end of the <see cref="BlockingQueue{T}"/>. The value can be a null reference (Nothing in Visual Basic) for reference types</param>
        public void Enqueue(T item)
        {
            this.CheckDisposed();
            _collection.Enqueue(item);
        }

        /// <summary>
        /// Tries to remove an item from the <see cref="BlockingQueue{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns><see langword="true"/>if an item was removed and returned from the beginning of the <see cref="BlockingQueue{T}"/>successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="BlockingQueue{T}" /> has been disposed.</exception>
        public bool TryDequeue(out T item)
        {
            return this.TryDequeue(out item, 0, CancellationToken.None);
        }

        /// <summary>
        /// Tries to remove an item from the <see cref="BlockingQueue{T}"/> in the specified time period.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="timeOut"></param>
        /// <returns><see langword="true"/>if an item was removed and returned from the beginning of the <see cref="BlockingQueue{T}"/>successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="BlockingQueue{T}" /> has been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeOut" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.-or-
        /// <paramref name="timeOut" /> is greater than <see cref="Int32.MaxValue" />.</exception>
        public bool TryDequeue(out T item, TimeSpan timeOut)
        {
            BlockingQueue<T>.ValidateTimeout(timeOut);
            return this.TryDequeue(out item, (int) timeOut.TotalMilliseconds, CancellationToken.None);
        }

        /// <summary>
        /// Tries to remove an item from the <see cref="BlockingQueue{T}"/> in the specified time period.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeOut"></param>
        /// <returns><see langword="true"/>if an item was removed and returned from the beginning of the <see cref="BlockingQueue{T}"/>successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="BlockingQueue{T}" /> has been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeOut" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        public bool TryDequeue(out T item, int millisecondsTimeOut)
        {
            BlockingQueue<T>.ValidateMillisecondsTimeout(millisecondsTimeOut);
            return this.TryDequeue(out item, millisecondsTimeOut, CancellationToken.None);
        }

        /// <summary>
        /// Tries to remove an item from the <see cref="BlockingQueue{T}"/> in the specified time period, while observing a cancellation token. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeOut"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see langword="true"/>if an item was removed and returned from the beginning of the <see cref="BlockingQueue{T}"/>successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="OperationCanceledException">The <see cref="CancellationToken" /> has been canceled.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="BlockingQueue{T}" /> has been disposed or the underlying <see cref="CancellationTokenSource" /> has been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeOut" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        public bool TryDequeue(out T item, int millisecondsTimeOut, CancellationToken cancellationToken)
        {
            BlockingQueue<T>.ValidateMillisecondsTimeout(millisecondsTimeOut);
            return this.TryDequeueWithNoTimeValidation(out item, millisecondsTimeOut, cancellationToken);
        }

        /// <summary>
        /// Tries to return an item from the beginning of the <see cref="BlockingQueue{T}"/> without removing it.
        /// </summary>
        /// <param name="item"></param>
        /// <returns><see langword="true" /> if an item was returned successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="BlockingQueue{T}" /> has been disposed.</exception>
        public bool TryPeek(out T item)
        {
            return this.TryPeek(out item, 0, CancellationToken.None);
        }

        /// <summary>
        /// Tries to return an item from the beginning of the <see cref="BlockingQueue{T}"/> in the specified time period without removing it.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="timeOut"></param>
        /// <returns><see langword="true" /> if an item was returned successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="BlockingQueue{T}" /> has been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeOut" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.-or-
        /// <paramref name="timeOut" /> is greater than <see cref="Int32.MaxValue" />.</exception>
        public bool TryPeek(out T item, TimeSpan timeOut)
        {
            BlockingQueue<T>.ValidateTimeout(timeOut);
            return this.TryPeek(out item, (int) timeOut.TotalMilliseconds, CancellationToken.None);
        }

        /// <summary>
        /// Tries to return an item from the beginning of the <see cref="BlockingQueue{T}"/> in the specified time period without removing it.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeOut"></param>
        /// <returns><see langword="true" /> if an item was returned successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="BlockingQueue{T}" /> has been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeOut" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        public bool TryPeek(out T item, int millisecondsTimeOut)
        {
            BlockingQueue<T>.ValidateMillisecondsTimeout(millisecondsTimeOut);
            return this.TryPeek(out item, millisecondsTimeOut, CancellationToken.None);
        }

        /// <summary>
        /// Tries to return an item from the beginning of the <see cref="BlockingQueue{T}"/> in the specified time period without removing it, while observing a cancellation token.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeOut"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see langword="true" /> if an item was returned successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="OperationCanceledException">The <see cref="CancellationToken" /> has been canceled.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="BlockingQueue{T}" /> has been disposed or the underlying <see cref="CancellationTokenSource" /> has been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeOut" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        public bool TryPeek(out T item, int millisecondsTimeOut, CancellationToken cancellationToken)
        {
            BlockingQueue<T>.ValidateMillisecondsTimeout(millisecondsTimeOut);
            return this.TryPeekWithNoTimeValidation(out item, millisecondsTimeOut, cancellationToken);
        }

        /// <summary>
        /// Copies the elements stored in the <see cref="BlockingQueue{T}"/> to a new array.
        /// </summary>
        /// <returns>A new array containing a snapshot of elements copied from the <see cref="BlockingQueue{T}"/>.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="BlockingQueue{T}" /> has been disposed.</exception>
        public T[] ToArray()
        {
            this.CheckDisposed();
            return this._collection.ToArray();
        }

        /// <summary>Copies the <see cref="BlockingQueue{T}" /> elements to an existing one-dimensional <see cref="Array" />, starting at the specified array index.</summary>
        /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from the <see cref="BlockingQueue{T}" />. The <see cref="Array" /> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="BlockingQueue{T}" /> has been disposed.</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array" /> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index" /> is less than zero.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index" /> is equal to or greater than the length of the <paramref name="array" /> -or- The number of elements in the source <see cref="BlockingQueue{T}" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
        public void CopyTo(T[] array, int index)
        {
            this.CheckDisposed();
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            this._collection.CopyTo(array, index);
        }

        private bool TryPeekWithNoTimeValidation(out T item, int millisecondsTimeout,
            CancellationToken cancellationToken)
        {
            this.CheckDisposed();
            item = default(T);

            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("The operation was canceled.", cancellationToken);
            }

            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken,
                this._blockinQueueCancellationTokenSource.Token);

            try
            {
                if (millisecondsTimeout != 0)
                {
                    cancellationTokenSource.CancelAfter(millisecondsTimeout);
                    while (true)
                    {
                        if (cancellationTokenSource.IsCancellationRequested)
                        {
                            throw new OperationCanceledException("The operation was canceled.",
                                cancellationTokenSource.Token);
                        }

                        if (_collection.TryPeek(out item))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException("The operation was canceled.", ex);
                }

                return false;
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (_collection.TryPeek(out item))
                {
                    return true;
                }
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException("The operation was canceled.", ex);
            }

            return false;
        }

        private bool TryDequeueWithNoTimeValidation(out T item, int millisecondsTimeout,
            CancellationToken cancellationToken)
        {
            this.CheckDisposed();
            item = default(T);

            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("The operation was canceled.", cancellationToken);
            }

            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken,
                this._blockinQueueCancellationTokenSource.Token);

            try
            {
                if (millisecondsTimeout != 0)
                {
                    cancellationTokenSource.CancelAfter(millisecondsTimeout);
                    while (true)
                    {
                        if (cancellationTokenSource.IsCancellationRequested)
                        {
                            throw new OperationCanceledException("The operation was canceled.",
                                cancellationTokenSource.Token);
                        }

                        if (_collection.TryDequeue(out item))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException("The operation was canceled.", ex);
                }

                return false;
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (_collection.TryDequeue(out item))
                {
                    return true;
                }
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException("The operation was canceled.", ex);
            }

            return false;
        }

        private static void ValidateTimeout(TimeSpan timeout)
        {
            long totalMilliseconds = (long) timeout.TotalMilliseconds;
            if ((totalMilliseconds < 0L || totalMilliseconds > (long) int.MaxValue) && totalMilliseconds != -1L)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout), timeout,
                    string.Format(CultureInfo.InvariantCulture,
                        $"The specified timeout must represent a value between {NON_BOUNDED} and {int.MaxValue}, inclusive."));
            }
        }

        private static void ValidateMillisecondsTimeout(int millisecondsTimeout)
        {
            if (millisecondsTimeout < 0 && millisecondsTimeout != -1)
            {
                throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout), millisecondsTimeout,
                    string.Format(CultureInfo.InvariantCulture,
                        $"The specified timeout must represent a value between {NON_BOUNDED} and {int.MaxValue}, inclusive."));
            }
        }

        /// <summary>Returns an enumerator that iterates through the <see cref="BlockingQueue{T}" />.</summary>
        /// <returns>An enumerator for the contents of the <see cref="BlockingQueue{T}" />.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            this.CheckDisposed();
            return this._collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>) this).GetEnumerator();
        }

        private void CheckDisposed()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException(nameof(BlockingQueue<T>), "The collection has been disposed.");
            }
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            this._serializationArray = this.ToArray();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            this.Initialize(new ConcurrentQueue<T>(_serializationArray));
            this._serializationArray = null;
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="BlockingQueue{T}"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize((object) this);
        }

        /// <summary>
        /// Releases resources used by the <see cref="BlockingQueue{T}"/> instance.
        /// </summary>
        /// <param name="disposing"></param>
        /// Whether being disposed explicitly (true) or due to a finalizer (false).
        protected virtual void Dispose(bool disposing)
        {
            if (this._isDisposed)
            {
                return;
            }

            this._blockinQueueCancellationTokenSource.Cancel();
            this._blockinQueueCancellationTokenSource.Dispose();
            this._isDisposed = true;
        }
    }
}
