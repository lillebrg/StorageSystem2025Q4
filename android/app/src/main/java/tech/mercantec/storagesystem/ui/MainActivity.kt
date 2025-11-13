package tech.mercantec.storagesystem.ui

import android.content.Intent
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.fragment.app.Fragment
import androidx.fragment.app.commit
import com.google.android.material.bottomnavigation.BottomNavigationView
import tech.mercantec.storagesystem.R

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main)

        val scanFragment = ScanFragment()
        val borrowedItemsFragment = BorrowedItemsFragment()

        loadFragment(scanFragment)

        findViewById<BottomNavigationView>(R.id.bottom_navbar).setOnItemSelectedListener {
            when (it.itemId) {
                R.id.scan -> {
                    loadFragment(scanFragment)
                }
                R.id.borrowed_items -> {
                    loadFragment(borrowedItemsFragment)
                }
            }
            true
        }

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main)) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }
    }

    private fun loadFragment(fragment: Fragment) {
        supportFragmentManager.commit {
            replace(R.id.container, fragment)
        }
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.app_bar_actions, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem) = when (item.itemId) {
        R.id.logout -> {
            // TODO handle logout

            val intent = Intent(this, LoginActivity::class.java)
            intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK // Prevent going back to authorized pages
            startActivity(intent)

            true
        }
        else -> super.onOptionsItemSelected(item)
    }
}
