using Xunit;
using static RailSim.Model.GraphExtensions;

namespace RailSim.Test
{
    public class BitMatrix_Test
    {
        [Fact]
        void _2x2()
        {
            CollisionMatrix matrix = new(2);

            Assert.True(matrix[0, 0]);
            Assert.True(matrix[1, 1]);

            Assert.False(matrix[1, 0]);
            Assert.False(matrix[0, 1]);

            matrix[1, 0] = true;
            Assert.True(matrix[1, 0]);
            Assert.True(matrix[0, 1]);
        }

        [Fact]
        void _3x3()
        {
            CollisionMatrix matrix = new(3);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i != j)
                    {
                        Assert.False(matrix[i, j]);
                    }
                    else Assert.True(matrix[i, j]);
                }
            }


            matrix[1, 0] = true;

            Assert.True(matrix[1, 0]);
            Assert.True(matrix[0, 1]);
        }

        [Fact]
        public void _10x10()
        {
            var matrix = new CollisionMatrix(10);

            matrix[9, 7] = true;
            Assert.True(matrix[7, 9]);
        }
    }
}
