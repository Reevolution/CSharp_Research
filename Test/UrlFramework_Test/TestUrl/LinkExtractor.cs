using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TestUrl
{
    public class LinkExtractor : IDisposable
    {
        private const int NON_BOUNDED = -1;

        public IEnumerable<string> Extract(string html)
        {
            var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (Match m in linkParser.Matches(html))
            {
                yield return m.Value;           
            }
        }

        private bool _isDisposed;
        private CancellationTokenSource _blockinQueueCancellationTokenSource;
        private WebClient _webClient;

        public LinkExtractor()
        {
            Initialize();
        }

        public bool TryGetUrl(out BaseUrlInfo item, string url, int millisecondsTimeOut, CancellationToken cancellationToken)
        {
            LinkExtractor.ValidateMillisecondsTimeout(millisecondsTimeOut);
            return this.TryGetUrlWithNoTimeValidation(out item, url, millisecondsTimeOut, cancellationToken);
        }

        private void Initialize()
        {
            _blockinQueueCancellationTokenSource = new CancellationTokenSource();
            _webClient = new WebClient();
        }

        private bool TryGetUrlWithNoTimeValidation(out BaseUrlInfo item, string url, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            this.CheckDisposed();
            item = null;

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

                        var buffer = _webClient.DownloadString(url);
                        var urlCount = Extract(buffer).Count();

                        item = new BaseUrlInfo() { Url = url, UrlCount = urlCount };

                        return true;
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

                var buffer = _webClient.DownloadString(url);
                var urlCount = Extract(buffer).Count();

                item = new BaseUrlInfo() { Url = url, UrlCount = urlCount };

                return true;
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException("The operation was canceled.", ex);
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

        private void CheckDisposed()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException(nameof(LinkExtractor), "The link extractor has been disposed.");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._isDisposed)
            {
                return;
            }

            this._isDisposed = true;
        }
    }
}
