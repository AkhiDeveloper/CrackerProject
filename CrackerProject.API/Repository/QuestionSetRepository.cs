using AutoMapper;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Model;
using Humanizer;
using MongoDB.Driver;
using ServiceStack;
using System.Linq.Expressions;

namespace CrackerProject.API.Repository
{
    public class QuestionSetRepository : BaseRepository<QuestionSet, DataModels.QuestionSet,Guid>, IQuestionSetRepository
    {
        private readonly IMongoCollection<DataModels.Section> _sections;
        public QuestionSetRepository(IMongoContext context, IMapper mapper) : base(context, mapper)
        {
            _sections = Context.GetCollection<DataModels.Section>
                (nameof(DataModels.Section).Pluralize());
        }

        public async Task AddtoSection(Guid sectionid, QuestionSet questionset)
        {
            var sectioncursor=await _sections.FindAsync(x=>x.Id == sectionid);
            var section = sectioncursor.FirstOrDefault();
            if(section == null)
            {
                throw new NullReferenceException
                    ("Section with id ={id} is not found.");
            }
            var questionsetdata = _mapper.Map<DataModels.QuestionSet>(questionset);
            if(section.QuestionSets == null)
            {
                section.QuestionSets = new List<DataModels.QuestionSet>();
            }
            section.QuestionSets.Add(questionsetdata);
            Context.AddCommand(() => _sections.ReplaceOneAsync(x => x.Id == sectionid, section));
        }

        public async Task<IEnumerable<QuestionSet>> GetfromSection(Guid sectionid)
        {
            var sectioncursor = await _sections.FindAsync(x => x.Id == sectionid);
            var section = sectioncursor.FirstOrDefault();
            if (section == null)
            {
                throw new NullReferenceException
                    ("Section with id ={id} is not found.");
            }
            var questionsetmodel = _mapper.Map<IEnumerable<QuestionSet>>(section.QuestionSets);
            return questionsetmodel;
        }

        public Task<IEnumerable<QuestionSet>> GetfromSection(Guid sectionid, Expression<Func<Section, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExistInSection(Guid sectionid, Guid questionset_id)
        {
            var sectioncursor = await _sections.FindAsync(x => x.Id == sectionid);
            var section = sectioncursor.FirstOrDefault();
            if (section == null)
            {
                throw new NullReferenceException
                    ($"Section with id ={sectionid} is not found.");
            }
            var isexist=section.QuestionSets.Any(x=>x.Id == questionset_id);
            return isexist;
        }

        public async Task MovetoSection(Guid question_id, Guid section_id)
        {
            var targetsectioncursor = await _sections.FindAsync(x => x.Id == section_id);
            var targetsection = targetsectioncursor.FirstOrDefault();
            if (targetsection == null)
            {
                throw new NullReferenceException
                    ($"Target Section with id ={section_id} is not found.");
            }
            if(targetsection.QuestionSets.Any(x=>x.Id == question_id))
            {
                return;
            }

            var originsectioncursor=await _sections.FindAsync(x=>
            x.QuestionSets.Any(q=>
            q.Id==question_id));
            var originsection = originsectioncursor.FirstOrDefault();
            if(originsection == null)
            {
                throw new NullReferenceException
                    ($"Questionset with id ={question_id} is not found.");
            }
            var questionsetdata = originsection.QuestionSets
                .FirstOrDefault(x => x.Id == question_id);
            if(!originsection.QuestionSets.Remove(questionsetdata))
            {
                throw new NullReferenceException
                    ($"Questionset with id ={question_id} is not found.");
            }
            targetsection.QuestionSets.Add(questionsetdata);
            Context.AddCommand(() => _sections
            .ReplaceOneAsync(x => x.Id == targetsection.Id, targetsection));
            Context.AddCommand(() => _sections
            .ReplaceOneAsync(x => x.Id == originsection.Id, targetsection));
        }

        public async Task RemoveSectionQuestionSets(Guid sectionid, DeleteType deleteType)
        {
            var originsectioncursor = await _sections
                .FindAsync(x => x.Id == sectionid);
            var originsection = originsectioncursor.FirstOrDefault();
            if (originsection == null)
            {
                throw new NullReferenceException
                    ($"Target Section with id ={sectionid} is not found.");
            }
            originsection.QuestionSets.Clear();
            Context.AddCommand(() => _sections.ReplaceOneAsync(
                x => x.Id == sectionid, originsection));
        }
    }
}
