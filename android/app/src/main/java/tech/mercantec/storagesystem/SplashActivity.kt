package tech.mercantec.storagesystem

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import tech.mercantec.storagesystem.services.Auth
import tech.mercantec.storagesystem.ui.LoginActivity
import tech.mercantec.storagesystem.ui.MainActivity

class SplashActivity : AppCompatActivity() {
    val auth = Auth(this)

    override fun onCreate(savedInstanceState: Bundle?) {
        if (auth.isLoggedIn()) {
            startActivity(Intent(this, MainActivity::class.java))

            val user = auth.getCurrentUser()
            Toast.makeText(this, "Welcome back, ${user.name}", Toast.LENGTH_SHORT).show()
        } else {
            startActivity(Intent(this, LoginActivity::class.java))
        }

        super.onCreate(savedInstanceState)
    }
}
