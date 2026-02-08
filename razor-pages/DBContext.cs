using Microsoft.Data.Sqlite;
using razor_pages.Structs;

namespace razor_pages.Pages;

public class DBContext : IDBContext
{
    private const string ConnectionString = "Data Source=minitwit.db";
    
    private SqliteConnection OpenConnection() {
        var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        return conn;
    }
    
    public User GetUserById(string id)
    {
        using var conn = OpenConnection();
        var cmd = conn.CreateCommand();
        
        cmd.CommandText = "SELECT * FROM user WHERE user_id = @userId";
        cmd.Parameters.AddWithValue("@userId", id);
        
        using (var reader = cmd.ExecuteReader()) {
            if (reader.Read())
            {
                return new User
                {
                    id = reader.GetInt32(reader.GetOrdinal("user_id")),
                    name = reader.GetString(reader.GetOrdinal("username")),
                    email = reader.GetString(reader.GetOrdinal("email"))
                };
            }
        }
        throw new Exception("Invald user_id");
    }

    public List<Message> GetPublicTimeline(int perPage)
    {
        using var conn = OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText =
            """
                SELECT 
                    m.message_id,
                    m.author_id,
                    m.text,
                    m.pub_date,
                    m.flagged,
                    
                    u.user_id,
                    u.username,
                    u.email
                    FROM message m 
                    JOIN user u ON m.author_id = u.user_id
                    WHERE m.flagged = 0
                    ORDER BY m.pub_date DESC
                    LIMIT @perpage
            """;
        cmd.Parameters.AddWithValue("@perpage", perPage);

        var timeline = new List<Message>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var user = new User
            {
                id = reader.GetInt32(reader.GetOrdinal("user_id")),
                name = reader.GetString(reader.GetOrdinal("username")),
                email = reader.GetString(reader.GetOrdinal("email"))
            };
            
            timeline.Add(
                new Message 
                {
                message_id = reader.GetInt32(reader.GetOrdinal("message_id")),
                author_id = reader.GetInt32(reader.GetOrdinal("author_id")),
                text = reader.GetString(reader.GetOrdinal("text")),
                pub_date = reader.GetString(reader.GetOrdinal("pub_date")),
                flagged = reader.GetString(reader.GetOrdinal("flagged")),
                author = user
            });
        }

        return timeline;
    }
    
    public List<Message> GetUserTimeline(int perPage, string username)
    {
        using var conn = OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText =
            """
                SELECT 
                    m.message_id,
                    m.author_id,
                    m.text,
                    m.pub_date,
                    m.flagged,
                    
                    u.user_id,
                    u.username,
                    u.email
                    FROM message m 
                    JOIN user u ON m.author_id = u.user_id
                    WHERE m.flagged = 0 AND u.username = @username
                    ORDER BY m.pub_date DESC
                    LIMIT @perpage
                 
            """;
        cmd.Parameters.AddWithValue("@perpage", perPage);
        cmd.Parameters.AddWithValue("@username", username);
        

		var timeline = new List<Message>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var user = new User
            {
                id = reader.GetInt32(reader.GetOrdinal("user_id")),
                name = reader.GetString(reader.GetOrdinal("username")),
                email = reader.GetString(reader.GetOrdinal("email"))
            };
            
            timeline.Add(
                new Message {
                message_id = reader.GetInt32(reader.GetOrdinal("message_id")),
                author_id = reader.GetInt32(reader.GetOrdinal("author_id")),
                text = reader.GetString(reader.GetOrdinal("text")),
                pub_date = reader.GetString(reader.GetOrdinal("pub_date")),
                flagged = reader.GetString(reader.GetOrdinal("flagged")),
                author = user
                });
        }

        return timeline;
    }
    public void CreateUser(string username, string email, string passwordHash)
    {
        using var conn = OpenConnection();
        var cmd = conn.CreateCommand();

        cmd.CommandText =
            """
            INSERT INTO user (username, email, pw_hash)
            VALUES (@username, @email, @pw_hash)
            """;

        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@pw_hash", passwordHash);

        cmd.ExecuteNonQuery();
    }
    public User GetUserByUsername(string username)
    {
        using var conn = OpenConnection();
        var cmd = conn.CreateCommand();
        
        cmd.CommandText = "SELECT * FROM user WHERE username = @username";
        cmd.Parameters.AddWithValue("@username", username);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new User
            {
                id = reader.GetInt32(reader.GetOrdinal("user_id")),
                name = reader.GetString(reader.GetOrdinal("username")),
                email = reader.GetString(reader.GetOrdinal("email"))
            };
        }
        return null;
    }
}