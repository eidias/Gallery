using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Service.Helpers
{
    public class FileSystemHelper
    {
        static readonly ConcurrentDictionary<string, FileSystemWatcher> FileSystemWatchers = new ConcurrentDictionary<string, FileSystemWatcher>();

        public static void AddWatcher(FileSystemWatcher fileSystemWatcher)
        {
            //You can set the buffer to 4 KB or larger, but it must not exceed 64 KB (underlying winapi restriction).
            fileSystemWatcher.InternalBufferSize = 1024 * 64;

            if (FileSystemWatchers.TryAdd(fileSystemWatcher.Filter, fileSystemWatcher))
            {
                return;
            }
            throw new InvalidOperationException("A watcher for this filter already exists.");
        }

        public static void RemoveWatcher(string filter)
        {
            FileSystemWatcher fileSystemWatcher;
            if(FileSystemWatchers.TryRemove(filter, out fileSystemWatcher))
            {
                return;
            }
            throw new InvalidOperationException("A watcher for this filter does not exist.");
        }

        public static void StartWatcher(string filter)
        {
            FileSystemWatchers[filter].EnableRaisingEvents = true;
        }

        public static void StopWatcher(string filter)
        {
            FileSystemWatchers[filter].EnableRaisingEvents = false;
        }
    }
}
