using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInteligente.servicios
{
    class FaceService
    {
        String FACE_SUBSCRIPTION_KEY = "686e3c49b4624325bfda8d3d2e4f03d8";
        String FACE_ENDPOINT = "https://nekliozface.cognitiveservices.azure.com/";

        // Usamos el Recognition Model 4 que salió en Febrero de 2021 ya que detecta caras con mascarillas mas fácilmente y es estable
        String RECOGNITION_MODEL4 = RecognitionModel.Recognition04;

        IFaceClient faceClient;

        public FaceService()
        {
            faceClient = FaceAuthenticate(FACE_ENDPOINT, FACE_SUBSCRIPTION_KEY);
        }

        public int ObtenerEdad(String imageURL)
        {
            // Creamos una lista de las caras que detectará
            IList<DetectedFace> detectedFaces;

            // Le hacemos reconocer las caras, esto puede tardar un poco
            detectedFaces = faceClient.Face.DetectWithUrlAsync(imageURL,
                returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Age },
                        detectionModel: DetectionModel.Detection01,
                        recognitionModel: RECOGNITION_MODEL4).Result;

            int edad = (int)detectedFaces[0].FaceAttributes.Age;
            // Devolvemos la edad de la cara principal
            return edad;
        }

        public string ObtenerGenero(string imageURL)
        {
            // Creamos una lista de las caras que detectará
            IList<DetectedFace> detectedFaces;

            // Le hacemos reconocer las caras, esto puede tardar un poco
            detectedFaces = faceClient.Face.DetectWithUrlAsync(imageURL,
                returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Gender },
                        detectionModel: DetectionModel.Detection01,
                        recognitionModel: RECOGNITION_MODEL4).Result;

            // Devolvemos el género de la cara principal
            string genero = detectedFaces[0].FaceAttributes.Gender.ToString() == "Male" ? "Hombre" : "Mujer";
            return genero;
        }


        private static IFaceClient FaceAuthenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }
    }
}

