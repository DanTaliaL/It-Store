using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class Options
    {
        public int Id { get; set; }
        //Processor
        public string ProcessorModelName { get; set; }
        public int QuantityCore { get; set; }
        public int NumberOfThreads { get; set; }
        public int CPUFrequency { get; set; }
        public int MaxCPUFrequency { get; set; }
        //Classification
        public string Model { get; set; }
        public string ManufacturerCode { get; set; }
        public string ReleaseYear { get; set; }
        public string OperatingSystem { get; set; }
        //Appearance
        public string CoverMaterial { get; set; }
        public string HousingMaterial { get; set; }
        //Screen
        public string ScreenType { get; set; }
        public string ScreenDiagonal { get; set; }
        public string ScreenResolution { get; set; }
        public string MaximumScreenRefreshRate { get; set; }
        public string PixelDensity { get; set; }
        //RAM
        public string RAMType { get; set; }
        public string RAMMemory { get; set; }
        public string RAMFrequency { get; set; }
        //graphics accelerator
        public string TypeOfGraphicsAccelerator { get; set; }
        public string BuiltInGraphicsCardModel { get; set; }
        public string DiscreteGraphicsCardModel { get; set; }
        //Data drives
        public string VolumeSSD { get; set; }
        public string VolumeHDD { get; set; }
        public string VolumeEMMC { get; set; }
       

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
