using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"),
                column: "Name",
                value: "Adventure travel");

            migrationBuilder.UpdateData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"),
                column: "Name",
                value: "Alternative energy");

            migrationBuilder.UpdateData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"),
                column: "Name",
                value: "Alternative medicine");

            migrationBuilder.InsertData(
                table: "Interests",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000004"), "Animal welfare" },
                    { new Guid("20000000-0000-0000-0000-000000000005"), "Astronomy" },
                    { new Guid("20000000-0000-0000-0000-000000000006"), "Athletics" },
                    { new Guid("20000000-0000-0000-0000-000000000007"), "Backpacking" },
                    { new Guid("20000000-0000-0000-0000-000000000008"), "Badminton" },
                    { new Guid("20000000-0000-0000-0000-000000000009"), "Baseball" },
                    { new Guid("20000000-0000-0000-0000-000000000010"), "Basketball" },
                    { new Guid("20000000-0000-0000-0000-000000000011"), "Beer tasting" },
                    { new Guid("20000000-0000-0000-0000-000000000012"), "Bicycling" },
                    { new Guid("20000000-0000-0000-0000-000000000013"), "Board games" },
                    { new Guid("20000000-0000-0000-0000-000000000014"), "Bowling" },
                    { new Guid("20000000-0000-0000-0000-000000000015"), "Brunch" },
                    { new Guid("20000000-0000-0000-0000-000000000016"), "Camping" },
                    { new Guid("20000000-0000-0000-0000-000000000017"), "Clubbing" },
                    { new Guid("20000000-0000-0000-0000-000000000018"), "Comedy" },
                    { new Guid("20000000-0000-0000-0000-000000000019"), "Conservation" },
                    { new Guid("20000000-0000-0000-0000-000000000020"), "Cooking" },
                    { new Guid("20000000-0000-0000-0000-000000000021"), "Crafts" },
                    { new Guid("20000000-0000-0000-0000-000000000022"), "DIY – Do it Yourself" },
                    { new Guid("20000000-0000-0000-0000-000000000023"), "Dancing" },
                    { new Guid("20000000-0000-0000-0000-000000000024"), "Dining out" },
                    { new Guid("20000000-0000-0000-0000-000000000025"), "Diving" },
                    { new Guid("20000000-0000-0000-0000-000000000026"), "Drinking" },
                    { new Guid("20000000-0000-0000-0000-000000000027"), "Education technology" },
                    { new Guid("20000000-0000-0000-0000-000000000028"), "Entrepreneurship" },
                    { new Guid("20000000-0000-0000-0000-000000000029"), "Environmental awareness" },
                    { new Guid("20000000-0000-0000-0000-000000000030"), "Fencing" },
                    { new Guid("20000000-0000-0000-0000-000000000031"), "Film" },
                    { new Guid("20000000-0000-0000-0000-000000000032"), "Finance technology" },
                    { new Guid("20000000-0000-0000-0000-000000000033"), "Fishing" },
                    { new Guid("20000000-0000-0000-0000-000000000034"), "Fitness" },
                    { new Guid("20000000-0000-0000-0000-000000000035"), "Frisbee" },
                    { new Guid("20000000-0000-0000-0000-000000000036"), "Gaming" },
                    { new Guid("20000000-0000-0000-0000-000000000037"), "Golf" },
                    { new Guid("20000000-0000-0000-0000-000000000038"), "Happy hour" },
                    { new Guid("20000000-0000-0000-0000-000000000039"), "Healing" },
                    { new Guid("20000000-0000-0000-0000-000000000040"), "Hiking" },
                    { new Guid("20000000-0000-0000-0000-000000000041"), "History" },
                    { new Guid("20000000-0000-0000-0000-000000000042"), "Holistic health" },
                    { new Guid("20000000-0000-0000-0000-000000000043"), "Horse riding" },
                    { new Guid("20000000-0000-0000-0000-000000000044"), "Human rights" },
                    { new Guid("20000000-0000-0000-0000-000000000045"), "Hunting" },
                    { new Guid("20000000-0000-0000-0000-000000000046"), "Ice skating" },
                    { new Guid("20000000-0000-0000-0000-000000000047"), "Innovation" },
                    { new Guid("20000000-0000-0000-0000-000000000048"), "International travel" },
                    { new Guid("20000000-0000-0000-0000-000000000049"), "Internet startups" },
                    { new Guid("20000000-0000-0000-0000-000000000050"), "Investing" },
                    { new Guid("20000000-0000-0000-0000-000000000051"), "Karaoke" },
                    { new Guid("20000000-0000-0000-0000-000000000052"), "Kayaking" },
                    { new Guid("20000000-0000-0000-0000-000000000053"), "Languages" },
                    { new Guid("20000000-0000-0000-0000-000000000054"), "Literature" },
                    { new Guid("20000000-0000-0000-0000-000000000055"), "Local culture" },
                    { new Guid("20000000-0000-0000-0000-000000000056"), "Marketing" },
                    { new Guid("20000000-0000-0000-0000-000000000057"), "Martial arts" },
                    { new Guid("20000000-0000-0000-0000-000000000058"), "Meditation" },
                    { new Guid("20000000-0000-0000-0000-000000000059"), "Mountain biking" },
                    { new Guid("20000000-0000-0000-0000-000000000060"), "Music" },
                    { new Guid("20000000-0000-0000-0000-000000000061"), "Natural parks" },
                    { new Guid("20000000-0000-0000-0000-000000000062"), "Networking" },
                    { new Guid("20000000-0000-0000-0000-000000000063"), "Neuroscience" },
                    { new Guid("20000000-0000-0000-0000-000000000064"), "Nightlife" },
                    { new Guid("20000000-0000-0000-0000-000000000065"), "Nutrition" },
                    { new Guid("20000000-0000-0000-0000-000000000066"), "Outdoor adventure" },
                    { new Guid("20000000-0000-0000-0000-000000000067"), "Outdoor sports" },
                    { new Guid("20000000-0000-0000-0000-000000000068"), "Painting" },
                    { new Guid("20000000-0000-0000-0000-000000000069"), "Photography" }
                });

            migrationBuilder.InsertData(
                table: "PersonalityTraits",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000003"), "Extrovert" },
                    { new Guid("30000000-0000-0000-0000-000000000004"), "Realistic" },
                    { new Guid("30000000-0000-0000-0000-000000000005"), "Ambitious" },
                    { new Guid("30000000-0000-0000-0000-000000000006"), "Easygoing" },
                    { new Guid("30000000-0000-0000-0000-000000000007"), "Thoughtful" },
                    { new Guid("30000000-0000-0000-0000-000000000008"), "Energetic" },
                    { new Guid("30000000-0000-0000-0000-000000000009"), "Creative" },
                    { new Guid("30000000-0000-0000-0000-000000000010"), "Reliable" },
                    { new Guid("30000000-0000-0000-0000-000000000011"), "Adventurous" },
                    { new Guid("30000000-0000-0000-0000-000000000012"), "Compassionate" }
                });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"),
                column: "Name",
                value: "A/B testing");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"),
                column: "Name",
                value: "AI");

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000003"), "API development" },
                    { new Guid("40000000-0000-0000-0000-000000000004"), "Accounting" },
                    { new Guid("40000000-0000-0000-0000-000000000005"), "Administrative support" },
                    { new Guid("40000000-0000-0000-0000-000000000006"), "Advertising" },
                    { new Guid("40000000-0000-0000-0000-000000000007"), "Affiliate marketing" },
                    { new Guid("40000000-0000-0000-0000-000000000008"), "Android development" },
                    { new Guid("40000000-0000-0000-0000-000000000009"), "Animators" },
                    { new Guid("40000000-0000-0000-0000-000000000010"), "Audio production" },
                    { new Guid("40000000-0000-0000-0000-000000000011"), "Back-end development" },
                    { new Guid("40000000-0000-0000-0000-000000000012"), "Blogging" },
                    { new Guid("40000000-0000-0000-0000-000000000013"), "Bookkeeping" },
                    { new Guid("40000000-0000-0000-0000-000000000014"), "Brand strategy" },
                    { new Guid("40000000-0000-0000-0000-000000000015"), "Branding" },
                    { new Guid("40000000-0000-0000-0000-000000000016"), "Business development" },
                    { new Guid("40000000-0000-0000-0000-000000000017"), "CRM management" },
                    { new Guid("40000000-0000-0000-0000-000000000018"), "Communication" },
                    { new Guid("40000000-0000-0000-0000-000000000019"), "Community management" },
                    { new Guid("40000000-0000-0000-0000-000000000020"), "Content" },
                    { new Guid("40000000-0000-0000-0000-000000000021"), "Content marketing" },
                    { new Guid("40000000-0000-0000-0000-000000000022"), "Copyediting" },
                    { new Guid("40000000-0000-0000-0000-000000000023"), "Copywriting" },
                    { new Guid("40000000-0000-0000-0000-000000000024"), "Creative writing" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000036"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000037"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000038"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000039"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000040"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000041"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000042"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000043"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000044"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000045"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000046"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000047"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000048"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000049"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000050"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000051"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000052"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000053"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000054"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000055"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000056"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000057"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000058"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000059"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000060"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000061"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000062"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000063"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000064"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000065"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000066"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000067"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000068"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000069"));

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "PersonalityTraits",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000024"));

            migrationBuilder.UpdateData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"),
                column: "Name",
                value: "Basketball");

            migrationBuilder.UpdateData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"),
                column: "Name",
                value: "Travel");

            migrationBuilder.UpdateData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"),
                column: "Name",
                value: "Photography");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"),
                column: "Name",
                value: "Web Development");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"),
                column: "Name",
                value: "Project Management");
        }
    }
}
