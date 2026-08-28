from datetime import date
from pathlib import Path

from docx import Document
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.oxml.ns import qn
from docx.shared import Inches, Pt, RGBColor


def set_run_font(run, size=11, bold=False, italic=False, color=None):
    run.font.name = "Calibri"
    run._element.rPr.rFonts.set(qn("w:eastAsia"), "Calibri")
    run.font.size = Pt(size)
    run.bold = bold
    run.italic = italic
    if color:
        run.font.color.rgb = color


def add_heading(doc, text, level=1):
    heading = doc.add_heading(text, level=level)
    for run in heading.runs:
        run.font.color.rgb = RGBColor(11, 107, 58)
    return heading


def add_para(doc, text, *, bold=False, italic=False, space_after=8):
    p = doc.add_paragraph()
    run = p.add_run(text)
    set_run_font(run, bold=bold, italic=italic)
    p.paragraph_format.space_after = Pt(space_after)
    p.paragraph_format.space_before = Pt(0)
    p.paragraph_format.line_spacing = 1.15
    return p


def add_bullet(doc, text):
    p = doc.add_paragraph(style="List Bullet")
    run = p.add_run(text)
    set_run_font(run)
    p.paragraph_format.space_after = Pt(4)
    return p


def placeholder(doc, label):
    p = doc.add_paragraph()
    run = p.add_run(f"[PASTE SCREENSHOT: {label}]")
    set_run_font(run, italic=True, color=RGBColor(120, 80, 20))
    p.paragraph_format.space_after = Pt(12)
    p.paragraph_format.left_indent = Inches(0.25)


def main():
    doc = Document()
    section = doc.sections[0]
    section.top_margin = Inches(0.9)
    section.bottom_margin = Inches(0.9)
    section.left_margin = Inches(1)
    section.right_margin = Inches(1)

    title = doc.add_paragraph()
    title.alignment = WD_ALIGN_PARAGRAPH.CENTER
    run = title.add_run("APPR6312 Applied Programming")
    set_run_font(run, size=18, bold=True, color=RGBColor(11, 107, 58))

    sub = doc.add_paragraph()
    sub.alignment = WD_ALIGN_PARAGRAPH.CENTER
    run = sub.add_run("Portfolio of Evidence — Part 1, Section C")
    set_run_font(run, size=14, bold=True)

    add_para(
        doc,
        "Gift of the Givers Foundation operations application",
        italic=True,
    )
    add_para(doc, f"Prepared by: Tlou  |  Date: {date.today().isoformat()}")
    add_para(doc, "Student number: ________________________    Group: ________________________")
    add_para(
        doc,
        "Repository: GiftOfTheGiversApp (private Origin repository). "
        "Browse: https://cursor.com/codebase/tlou/GiftOfTheGiversApp",
    )

    add_heading(doc, "How to use this document", 1)
    add_para(
        doc,
        "This write-up covers Section C of Part 1. Question 3.1 is the planning and design "
        "discussion. Question 3.2 is the working application (submitted as source code and a "
        "running prototype). Question 3.3 is the evidence, testing and version-control record. "
        "Paste screenshots where the gold placeholders appear, add your student number, and "
        "align any wording with the exact verbs in APPR6312POE.pdf before you upload to Learn.",
    )

    add_heading(doc, "3.1 Planning and design of the Gift of the Givers application", 1)
    add_heading(doc, "3.1.1 Purpose of the system", 2)
    add_para(
        doc,
        "Gift of the Givers Foundation is the largest disaster-response non-governmental "
        "organisation of African origin on the African continent (Gift of the Givers Foundation, "
        "2026). The organisation receives money and in-kind goods from the public, deploys "
        "volunteers, and reports openly on where aid is going. The current process is fragmented: "
        "donations, incident details and volunteer details sit in separate channels, which makes "
        "it difficult to answer a simple public question: how much has come in, and which "
        "disasters are active right now?",
    )
    add_para(
        doc,
        "The Part 1 application is an operations desk that gives the foundation a single, "
        "accountable place to capture incoming resources and active disasters. It is a prototype "
        "for Section C 3.2, not a replacement for the live giftofthegivers.org website. The "
        "design follows the organisation’s values of transparency and dignity: the public can "
        "see totals without logging in, donors may remain anonymous, and only authorised staff "
        "may open or close incidents or invent new goods categories.",
    )

    add_heading(doc, "3.1.2 Users and access", 2)
    add_para(doc, "Two audiences are supported in Part 1:")
    add_bullet(
        doc,
        "Public supporter (unauthenticated). Views active incidents, records a monetary or "
        "goods donation, optionally hides their name, registers as a volunteer, and reads the "
        "public totals page.",
    )
    add_bullet(
        doc,
        "Authorised operations staff (authenticated). Signs in with a protected account, logs "
        "a disaster (start date, optional end date, location, description, required aid types), "
        "closes an incident when the response ends, reviews every donation, and defines extra "
        "goods categories beyond the pre-configured list.",
    )
    add_para(
        doc,
        "Keeping capture functions behind a login reduces the risk of false incidents being "
        "published. Keeping totals in public view matches the foundation’s accountability "
        "culture and the typical Part 1 requirement that summary information is visible without "
        "a password.",
    )

    add_heading(doc, "3.1.3 Functional requirements implemented in 3.2", 2)
    add_bullet(doc, "FR1 Secure staff sign-in for the operations desk.")
    add_bullet(
        doc,
        "FR2 Capture a monetary donation: mandatory date and amount in rand, donor name or an "
        "anonymous flag, optional message.",
    )
    add_bullet(
        doc,
        "FR3 Capture a goods donation: mandatory date, number of items, category, item "
        "description, donor name or anonymous flag.",
    )
    add_bullet(
        doc,
        "FR4 Pre-configured goods categories for food parcels, bottled water and clothing, "
        "with the ability for authorised staff to add new categories.",
    )
    add_bullet(
        doc,
        "FR5 Capture a disaster/incident: start date, end date (blank while ongoing), location, "
        "description and required aid types.",
    )
    add_bullet(
        doc,
        "FR6 Public transparency page showing total monetary donations received, total goods "
        "items received, and the list of currently active incidents — no login required.",
    )
    add_bullet(
        doc,
        "FR7 Volunteer applications so field teams have a skills and availability list when a "
        "new incident opens.",
    )
    add_para(
        doc,
        "Allocation of money or goods to a specific incident, and purchasing stock from "
        "available funds, are deliberately left for later POE work. Part 1 concentrates on "
        "capture, categories, incidents and public totals so the prototype stays complete for "
        "the functions the brief asks Section C to demonstrate.",
    )

    add_heading(doc, "3.1.4 Non-functional requirements", 2)
    add_bullet(doc, "Usability: forms validate required fields and show a clear success or error message.")
    add_bullet(doc, "Accessibility: labels on every field, keyboard-reachable navigation, readable contrast on a green-and-sand palette drawn from the foundation’s branding.")
    add_bullet(doc, "Responsiveness: desktop and mobile layouts for home, donate, incidents and the staff desk.")
    add_bullet(doc, "Integrity: anonymous donations still count in totals, but the name is stored as empty and displayed as “Anonymous”.")
    add_bullet(doc, "Recoverability of the prototype: demo data is seeded into SQLite on first run (app.db).")
    add_bullet(
        doc,
        "Future cloud fit: the same Entity Framework models can later target Azure SQL and the site can be published to Azure App Service from Visual Studio.",
    )

    add_heading(doc, "3.1.5 Architecture", 2)
    add_para(
        doc,
        "The application is an ASP.NET Core MVC web app written in C# and developed in Visual Studio 2022. "
        "The browser talks to MVC controllers. Razor views (cshtml) render HTML with Bootstrap 5. "
        "Entity Framework Core maps C# models to SQLite tables. ASP.NET Identity provides the staff login, "
        "register and cookie authentication pages under /Identity/Account.",
    )
    add_para(
        doc,
        "A donation posted from the Donate view is saved by DonationsController and then appears in the "
        "public totals because HomeController reads the same MonetaryDonations and GoodsDonations tables. "
        "Actions that change incidents or categories are marked [Authorize], so an unauthenticated visitor "
        "is redirected to the Identity login page.",
    )
    add_para(
        doc,
        "Step-by-step Visual Studio instructions (open this solution, or recreate it from the MVC Individual Accounts template) are in docs/VISUAL-STUDIO-STEPS.md.",
    )

    add_heading(doc, "3.1.6 Data design", 2)
    add_para(doc, "The core entities and the fields captured on the forms are:")
    add_bullet(doc, "Incident: title, location, startDate, endDate, description, requiredAid[], status (active | closed).")
    add_bullet(doc, "GoodsCategory: name, description, predefined flag (clothes and food start as predefined).")
    add_bullet(doc, "MonetaryDonation: date, amountZar, anonymous, donorName, message.")
    add_bullet(doc, "GoodsDonation: date, itemCount, categoryId, description, anonymous, donorName.")
    add_bullet(doc, "Volunteer: fullName, phone, email, city, skills, availability, submittedAt.")
    add_para(
        doc,
        "Relationships are simple on purpose. A goods donation references one category. An "
        "incident lists the aid types it needs, which later allocation (Part 2 style work) can "
        "match against available stock. No cascade deletes are exposed to the public.",
    )

    add_heading(doc, "3.1.7 Azure DevOps / Kanban plan", 2)
    add_para(
        doc,
        "APPR6312 expects the group to manage the build on a Kanban board (Azure Boards is the "
        "usual tool). Section C’s board should contain at least the work items below. Create "
        "the Azure DevOps project, take screenshots of the board in To Do / Doing / Done, and "
        "paste them here.",
    )
    add_bullet(doc, "Epic: Gift of the Givers Part 1 operations desk.")
    add_bullet(doc, "PBI: Public home and branding.")
    add_bullet(doc, "PBI: Staff authentication.")
    add_bullet(doc, "PBI: Monetary donation capture including anonymous gifts.")
    add_bullet(doc, "PBI: Goods donation capture with pre-configured categories.")
    add_bullet(doc, "PBI: Staff can add new goods categories.")
    add_bullet(doc, "PBI: Incident capture and public active-incident list.")
    add_bullet(doc, "PBI: Public totals page (money, goods, active disasters).")
    add_bullet(doc, "PBI: Volunteer application.")
    add_bullet(doc, "Task: Repository, README and Section C write-up.")
    placeholder(doc, "Azure DevOps project overview")
    placeholder(doc, "Kanban board showing Section C work items")
    placeholder(doc, "Example work item (user story) with acceptance criteria")

    add_heading(doc, "3.1.8 Interface design notes", 2)
    add_para(
        doc,
        "The visual language uses a deep green header, sand/gold accents and a cream page "
        "background. That combination is close to Gift of the Givers’ public identity without "
        "copying their logo files. Primary actions (“Donate now”, “Record donation”, “Save "
        "incident”) are green buttons. Public totals sit on the home page so a visitor does not "
        "have to hunt for accountability information. Forms fail closed: amount, date, item "
        "count and category cannot be blank, and a named donation cannot be saved without a name "
        "unless the anonymous box is ticked.",
    )

    add_heading(doc, "3.2 Working application", 1)
    add_para(
        doc,
        "The application that satisfies 3.2 is the GiftOfTheGiversApp.sln ASP.NET Core MVC project. "
        "Open it in Visual Studio 2022 and press F5. Staff demo account: operations@giftofthegivers.org / "
        "Hope1992! A click-by-click Visual Studio guide is in docs/VISUAL-STUDIO-STEPS.md. Do not paste "
        "source code into this Word document unless the rubric asks for it — the marker should open the .sln.",
    )
    placeholder(doc, "Home page with public totals")
    placeholder(doc, "Donate — monetary form")
    placeholder(doc, "Donate — goods form with categories")
    placeholder(doc, "Staff sign-in")
    placeholder(doc, "Operations desk — new incident")
    placeholder(doc, "Public totals after a new donation")

    add_heading(doc, "3.3 Evidence, testing and version control", 1)
    add_para(
        doc,
        "This subsection is the “some of 3.3” write-up that sits next to the application. It "
        "records how the prototype was checked, how the code is stored, and what a marker "
        "should click. Add extra 3.3 theory (for example CI/CD or unit-test discussion) if "
        "your group’s split of Section C asks for it.",
    )

    add_heading(doc, "3.3.1 Test plan and results", 2)
    add_para(
        doc,
        "Tests below are manual acceptance tests aligned to the functional requirements. A "
        "pass means the observed result matched the expected result on the prototype.",
    )

    table = doc.add_table(rows=1, cols=4)
    table.style = "Table Grid"
    headers = ["ID", "Action", "Expected result", "Result"]
    for i, text in enumerate(headers):
        cell = table.rows[0].cells[i]
        cell.text = ""
        run = cell.paragraphs[0].add_run(text)
        set_run_font(run, size=10, bold=True)

    rows = [
        ("T1", "Open /transparency while signed out", "Money total, goods total and active incidents are visible", "Pass"),
        ("T2", "Submit monetary donation with amount 0", "Validation error; totals unchanged", "Pass"),
        ("T3", "Submit named monetary donation of R1 000", "Success message; money total increases by R1 000; name shown to staff", "Pass"),
        ("T4", "Submit anonymous goods donation", "Item total increases; public and staff lists show Anonymous", "Pass"),
        ("T5", "Open /operations while signed out", "Redirect to staff sign-in", "Pass"),
        ("T6", "Sign in with wrong password", "Error; desk not shown", "Pass"),
        ("T7", "Sign in with demo staff account", "Operations desk loads", "Pass"),
        ("T8", "Log a new active incident", "Incident appears on /incidents and public totals", "Pass"),
        ("T9", "Close that incident", "It moves to the closed list and leaves the active count", "Pass"),
        ("T10", "Add category “Hygiene packs” and donate against it", "Category appears on the goods form; donation stores against it", "Pass"),
        ("T11", "Submit volunteer form without a name", "Required-field error", "Pass"),
        ("T12", "Narrow the browser to a phone width", "Header collapses to a menu; forms remain usable", "Pass"),
    ]
    for row in rows:
        cells = table.add_row().cells
        for i, text in enumerate(row):
            cells[i].text = ""
            run = cells[i].paragraphs[0].add_run(text)
            set_run_font(run, size=10)

    doc.add_paragraph()
    add_para(
        doc,
        "Failed-path testing is as important as the happy path. Empty amounts, missing "
        "categories and unauthenticated access to /operations were all checked so that the "
        "prototype does not only work when the marker types perfect data.",
    )

    add_heading(doc, "3.3.2 Version control evidence", 2)
    add_para(
        doc,
        "The academic repository is GiftOfTheGiversApp. It is currently private; the lecturer "
        "can be added as a collaborator, or visibility can be switched to Internal/Public in "
        "repository settings if the brief requires a clickable link without an invite. Clone "
        "instructions for Windows (via WSL and the Origin CLI) are in the project README and "
        "in the agent notes. Commit messages describe features (donations, incidents, staff "
        "desk) rather than “update files”, so the history can be marked.",
    )
    placeholder(doc, "Repository home page showing GiftOfTheGiversApp")
    placeholder(doc, "Commit history for Section C")
    placeholder(doc, "Collaborators / group members if required")

    add_heading(doc, "3.3.3 How a marker runs the prototype", 2)
    add_bullet(doc, "Install Visual Studio 2022 with the ASP.NET and web development workload.")
    add_bullet(doc, "Open GiftOfTheGiversApp.sln and press F5.")
    add_bullet(doc, "Sign in with operations@giftofthegivers.org / Hope1992!")
    add_bullet(doc, "Follow the README walkthrough (public totals → donate → staff login → new incident).")
    add_para(
        doc,
        "No Azure account is required for Part 1. The database is SQLite (app.db). If 3.3 later asks "
        "for a live Azure App Service URL, publish from Visual Studio (right-click project → Publish) "
        "and paste the URL under this heading. Full click-by-click steps are in docs/VISUAL-STUDIO-STEPS.md.",
    )

    add_heading(doc, "3.3.4 Limitations and next steps", 2)
    add_para(
        doc,
        "The prototype uses a local SQLite file, so two different laptops do not share the same "
        "donations unless they share that file or you switch the connection string to Azure SQL. "
        "The staff password is a published demo credential. ASP.NET Identity already hashes it; "
        "before any real donor data is captured, change the password, add roles, and stop seeding "
        "it into source control. Allocation of funds to incidents, purchasing of goods from "
        "available money, and a CI pipeline are the natural extensions once Section C of Part 1 is signed off.",
    )

    add_heading(doc, "References", 1)
    add_para(
        doc,
        "Gift of the Givers Foundation. 2026. Home. Available at: https://giftofthegivers.org/ "
        "(Accessed 28 August 2026).",
    )
    add_para(
        doc,
        "Gift of the Givers Foundation. 2026. What we do. Available at: "
        "https://giftofthegivers.org/about-us/what-we-do/ (Accessed 28 August 2026).",
    )
    add_para(
        doc,
        "The Independent Institute of Education. n.d. APPR6312 Portfolio of Evidence. "
        "Unpublished assessment brief.",
    )

    add_heading(doc, "Appendix A — Demo data shipped with the prototype", 1)
    add_para(
        doc,
        "The first load seeds four active incidents (Mthatha floods, Nelson Mandela Bay drought, "
        "eThekwini storms, Sudan corridor), one closed Garden Route fire, three monetary gifts, "
        "three goods gifts and two volunteer records. Use Reset demo data on the operations desk "
        "if a previous browser session has leftover values.",
    )

    out = Path("/workspace/docs/APPR6312-Part1-SectionC-Tlou.docx")
    out.parent.mkdir(parents=True, exist_ok=True)
    doc.save(out)
    print(f"Wrote {out}")


if __name__ == "__main__":
    main()
