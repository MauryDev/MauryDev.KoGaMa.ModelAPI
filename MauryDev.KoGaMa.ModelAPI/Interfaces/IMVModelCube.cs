using MauryDev.KoGaMa.ModelAPI.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauryDev.KoGaMa.ModelAPI.Interfaces
{
    public interface IMVModelCube
    {
        void AddCube(Cube cube);
        void RemoveCube(IntVector position);
        void UpdateCube(Cube cube);
        void HandleDelta();
        IEnumerable<Cube> GetCubes();
        Cube GetCube(IntVector position);
        bool ContainsCube(IntVector position);

        bool ContainsCubes { get; }

        int CubeCount { get; }

        float PrototypeScale { get; }

        int Pid { get; }
        int Id { get; }

        int AuthorProfileID { get; }
    }
}
