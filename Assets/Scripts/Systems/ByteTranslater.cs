using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Systems {
    public class ByteTranslater {

        public static byte[] CipherIntToByte(IEnumerable<int> ints) {
            var res = new List<byte>();

            foreach (var item in ints) {
                res.AddRange(BitConverter.GetBytes(item));
            }
            return res.ToArray();
        }

        public static int[] DecodeByteToInt(IEnumerable<byte> bytes) {
            var res=new List<int>();
            var size = sizeof(int);
            for(var i=0;i<bytes.Count();i+=size) {
                var temp = bytes.Skip(i).Take(size);
                res.Add(BitConverter.ToInt32(temp.ToArray(),0));
            }

            return res.ToArray();
        }


    }
}