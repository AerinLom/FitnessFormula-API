using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FitnessFormulaMVP.Models
{
    public class WorkoutViewModel
    {
        public List<WorkoutModel> Workouts { get; set; }
    }
    public class WorkoutModel
    {
        public int WorkoutId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public int Duration { get; set; }
        public List<ExerciseModel> Exercises { get; set; }
    }
    public class CustomWorkoutConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<WorkoutModel>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var workouts = new List<WorkoutModel>();
            var jsonObject = JObject.Load(reader);

            foreach (var item in jsonObject["$values"])
            {
                var workout = item.ToObject<WorkoutModel>();
                var exercises = item["Exercises"]["$values"]?.ToObject<List<ExerciseModel>>() ?? new List<ExerciseModel>();
                workout.Exercises = exercises;
                workouts.Add(workout);
            }

            return workouts;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
